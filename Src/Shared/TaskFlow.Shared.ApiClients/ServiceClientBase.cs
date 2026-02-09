using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.CircuitBreaker;
using System.Net;
using System.Net.Http.Json;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Shared.ApiClients {
    public class ServiceClientBase {
        protected readonly ILogger _logger;
        protected readonly HttpClient _httpClient;
        protected readonly ResiliencePipeline<HttpResponseMessage> _resiliencePipeline;

        protected ServiceClientBase(ILogger logger, HttpClient httpClient, ResiliencePipeline<HttpResponseMessage>? resiliencePipeline = null) {
            _logger = logger;
            _httpClient = httpClient;

            _resiliencePipeline = resiliencePipeline ?? CreateDefaultResiliencePipeline();
        }

        protected static ResiliencePipeline<HttpResponseMessage> CreateDefaultResiliencePipeline() {
            return new ResiliencePipelineBuilder<HttpResponseMessage>()
                    .AddTimeout(TimeSpan.FromSeconds(15))
                    .AddRetry(new RetryStrategyOptions<HttpResponseMessage>() {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(r => (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout),
                        MaxRetryAttempts = 3,
                        Delay = TimeSpan.FromSeconds(2),
                        BackoffType = DelayBackoffType.Exponential
                    })
                    .AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>() {
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<HttpRequestException>()
                            .HandleResult(r => (int)r.StatusCode >= 500),
                        FailureRatio = 0.5,
                        SamplingDuration = TimeSpan.FromSeconds(30),
                        MinimumThroughput = 5,
                        BreakDuration = TimeSpan.FromSeconds(30)
                    })
                    .Build();
        }

        protected async Task<TResult?> ExecuteGetAsync<TResult>(string endpoint, CancellationToken ct = default) {
            var context = ResilienceContextPool.Shared.Get(ct);
            
            try {
                var response = await _resiliencePipeline.ExecuteAsync(
                    async (ctx, state) => {
                        var (client, ep, log) = state;
                        return await client.GetAsync(ep, ctx.CancellationToken).ConfigureAwait(false);
                    },
                    context,
                    (_httpClient, endpoint, _logger))
                    .ConfigureAwait(false);

                return await ProcessResponseAsync<TResult>(response, endpoint, ct).ConfigureAwait(false);
            } catch (HttpRequestException ex) {
                _logger.Error($"Network error for {endpoint}", ex);
                throw new HttpRequestException($"Service unavailable: {endpoint}", ex);
            } catch (TimeoutRejectedException ex) {
                _logger.Warn($"Timeout for {endpoint}");
                throw new TimeoutException($"Service timeout: {endpoint}", ex);
            } catch (BrokenCircuitException ex) {
                _logger.Error($"Circuit breaker open for {endpoint}", ex);
                throw new InvalidOperationException(
                    $"Service temporarily unavailable: {endpoint}", ex);
            } catch (Exception ex) when (ex is not OperationCanceledException) {
                _logger.Error($"Unexpected error for {endpoint}", ex);
                throw;
            } finally {
                ResilienceContextPool.Shared.Return(context);
            }
        }

        private async Task<TResult?> ProcessResponseAsync<TResult>(HttpResponseMessage response, string endpoint, CancellationToken ct) {
            if (response.IsSuccessStatusCode) {
                try {
                    return await response.Content
                        .ReadFromJsonAsync<TResult>(cancellationToken: ct)
                        .ConfigureAwait(false);
                } catch (Exception ex) {
                    _logger.Error($"Failed to deserialize response from {endpoint}", ex);
                    throw new InvalidOperationException($"Failed to deserialize response from {endpoint}", ex);
                }
            }

            var statusCode = (int)response.StatusCode;

            if (response.StatusCode == HttpStatusCode.NotFound) {
                _logger.Debug($"Resource not found: {endpoint}");
                return default;
            }

            try {
                var content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                _logger.Warn($"Service error {statusCode} for {endpoint}: {content}");
            } catch (Exception logEx) {
                _logger.Warn($"Service error {statusCode} for {endpoint}. Failed to read response: {logEx.Message}");
            }

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500) {
                throw new HttpRequestException(
                    $"Client error {statusCode} for {endpoint}",
                    null,
                    response.StatusCode);
            }

            response.EnsureSuccessStatusCode();
            return default;
        }
    }
}
