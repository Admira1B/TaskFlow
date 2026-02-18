using TaskFlow.Shared.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskFlow.Shared.Core.Abstractions {
    public abstract class ServiceHealthCheckBase : IHealthCheck {
        private readonly ILogger _logger;
        private readonly string _serviceName;
        private readonly DateTime _startTime;

        protected ServiceHealthCheckBase(ILogger logger, string serviceName) {
            _logger = logger;
            _serviceName = serviceName;
            _startTime = DateTime.UtcNow;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                var serviceCheckResult = await CheckServiceHealthAsync(cancellationToken);
                if (serviceCheckResult.Status != HealthStatus.Healthy) {
                    return serviceCheckResult;
                }

                var dependenciesCheckResult = await CheckDependenciesHealthAsync(context, cancellationToken);
                if (dependenciesCheckResult.Status != HealthStatus.Healthy) {
                    return dependenciesCheckResult;
                }

                return await BuildHealthyResultAsync(cancellationToken);
            } catch (Exception ex) {
                return await HandleExceptionAsync(cancellationToken, ex);
            }
        }

        protected virtual Task<HealthCheckResult> CheckServiceHealthAsync(CancellationToken cancellationToken) {
            var uptime = DateTime.UtcNow - _startTime;

            if (uptime.TotalSeconds < 0) {
                return Task.FromResult(
                        HealthCheckResult.Unhealthy(
                            description: "System clock inconsistency detected",
                            data: new Dictionary<string, object> {
                                ["error"] = "Invalid uptime calculation",
                                ["start_time"] = _startTime,
                                ["current_time"] = DateTime.UtcNow
                            }
                        )
                );
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }

        protected virtual Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            // This method can be override with added Dependencies

            return Task.FromResult(HealthCheckResult.Healthy());
        }

        protected virtual Task<HealthCheckResult> BuildHealthyResultAsync(CancellationToken cancellationToken) {
            var uptime = DateTime.UtcNow - _startTime;

            var data = new Dictionary<string, object> {
                ["status"] = "Healthy",
                ["service"] = _serviceName,
                ["current_time"] = DateTime.UtcNow,
                ["start_time"] = _startTime,
                ["uptime_human"] = FormatUptime(uptime),
                ["uptime_seconds"] = Math.Round(uptime.TotalSeconds, 2),
            };

            _logger.Debug(
                "{ServiceName} health check passed. Uptime: {Uptime}",
                _serviceName,
                FormatUptime(uptime)
            );

            return Task.FromResult(
                HealthCheckResult.Healthy(
                    description: $"{_serviceName} is healthy. Uptime: {FormatUptime(uptime)}",
                    data: data
                )
            );
        }

        protected virtual Task<HealthCheckResult> HandleExceptionAsync(CancellationToken cancellationToken, Exception ex) {
            _logger.Error(
                "{ServiceName} health check failed with exception", ex, _serviceName);

            var data = new Dictionary<string, object> {
                ["service"] = _serviceName,
                ["timestamp"] = DateTime.UtcNow,
                ["error_message"] = ex.Message,
                ["error_type"] = ex.GetType().Name,
                ["stack_trace"] = ex.StackTrace ?? string.Empty
            };

            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    description: $"{_serviceName} health check failed: {ex.Message}",
                    exception: ex,
                    data: data
                )
            );
        }

        private static string FormatUptime(TimeSpan uptime) {
            if (uptime.TotalDays >= 1)
                return $"{uptime.TotalDays:F1} days";
            if (uptime.TotalHours >= 1)
                return $"{uptime.TotalHours:F1} hours";
            if (uptime.TotalMinutes >= 1)
                return $"{uptime.TotalMinutes:F1} minutes";

            return $"{uptime.TotalSeconds:F0} seconds";
        }
    }
}
