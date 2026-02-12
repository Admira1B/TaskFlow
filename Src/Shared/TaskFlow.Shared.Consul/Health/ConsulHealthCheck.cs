using Consul;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Shared.Consul.Health {
    public class ConsulHealthCheck(ILogger logger, IOptions<ConsulOptions> options, IConsulClient consulClient) : IHealthCheck {
        private readonly ILogger _logger = logger;
        private readonly ConsulOptions _options = options.Value;
        private readonly IConsulClient _consulClient = consulClient;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                var leader = await _consulClient.Status.Leader(cancellationToken);

                if (string.IsNullOrEmpty(leader)) {
                    _logger.Warn("Consul has no leader");
                    return HealthCheckResult.Degraded("Consul has no leader");
                }

                var self = await _consulClient.Agent.Self(cancellationToken);
                var member = self.Response["Member"];
                var status = member?["Status"]?.ToString();

                // '1' is equal to 'alive' status
                if (status != "1" && status != "alive") 
                {
                    _logger.Warn($"Consul agent status: {status}");
                    return HealthCheckResult.Degraded($"Consul agent status: {status}");
                }

                var datacenters = await _consulClient.Catalog.Datacenters(cancellationToken);

                var data = new Dictionary<string, object> {
                    ["leader"] = leader,
                    ["datacenter"] = _options.Datacenter,
                    ["address"] = _options.Address,
                    ["datacenters"] = string.Join(",", datacenters),
                    ["agent_status"] = status ?? "unknown"
                };

                _logger.Debug($"Consul health check passed. Leader: {leader}");

                return HealthCheckResult.Healthy();
            } catch (HttpRequestException ex) {
                _logger.Error($"Consul HTTP request failed", ex);
                return HealthCheckResult.Unhealthy(
                    $"Cannot connect to Consul at {_options.Address}", ex);
            } catch (TimeoutException ex) {
                _logger.Error($"Consul connection timeout", ex);
                return HealthCheckResult.Unhealthy($"Connection timeout to Consul at {_options.Address}", ex);
            } catch (Exception ex) {
                _logger.Error($"Unexpected error checking Consul health", ex);
                return HealthCheckResult.Unhealthy($"Failed to check Consul health: {ex.Message}", ex);
            }
        }
    }
}
