using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskFlow.Gateway.HealthChecks {
    public class GatewayHealthCheck(Shared.Core.Interfaces.ILogger logger) : IHealthCheck {
        private readonly Shared.Core.Interfaces.ILogger _logger = logger;
        private static readonly DateTime _startTime = DateTime.UtcNow;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default) {
            try {
                var uptime = DateTime.UtcNow - _startTime;

                var data = new Dictionary<string, object> {
                    ["uptime"] = uptime.TotalSeconds,
                    ["start_time"] = _startTime,
                    ["current_time"] = DateTime.UtcNow,
                    ["status"] = "Running"
                };

                _logger.Debug("Gateway health check executed - Status: Healthy, Uptime: {Uptime}s", uptime.TotalSeconds.ToString());

                return Task.FromResult(
                    HealthCheckResult.Healthy(description: $"Gateway is running. Uptime: {uptime.TotalSeconds:F0} seconds", data: data)
                );
            } catch (Exception ex) {
                _logger.Error("Gateway health check failed", ex);

                return Task.FromResult(
                    HealthCheckResult.Unhealthy(
                        description: "Gateway health check failed",
                        exception: ex,
                        data: new Dictionary<string, object> {
                            ["error"] = ex.Message,
                            ["timestamp"] = DateTime.UtcNow
                        }
                    )
                );
            }
        }
    }
}
