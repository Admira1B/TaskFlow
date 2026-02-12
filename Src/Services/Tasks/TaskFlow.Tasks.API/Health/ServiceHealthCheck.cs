using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Core.Health;
using TaskFlow.Shared.Messaging.Health;
using TaskFlow.Tasks.Infrastructure.SqlServer.Health;

namespace TaskFlow.Tasks.API.Health {
    public class ServiceHealthCheck(Shared.Core.Interfaces.ILogger logger, DataBaseHealthCheck dataBaseHealth, RabbitMqHealthCheck rabbitMqHealth) :
             ServiceHealthCheckBase(logger, "Tasks Service") {
        private readonly DataBaseHealthCheck _dataBaseHealth = dataBaseHealth;
        private readonly RabbitMqHealthCheck _rabbitMqHealth = rabbitMqHealth;

        protected override async Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            var checks = new List<Task<HealthCheckResult>> {
                _dataBaseHealth.CheckHealthAsync(context, cancellationToken),
                _rabbitMqHealth.CheckHealthAsync(context, cancellationToken),
            };

            var results = await Task.WhenAll(checks);

            var unhealthyResults = results.Where(r => r.Status != HealthStatus.Healthy).ToList();

            if (unhealthyResults.Count is not 0) {
                var errors = string.Join("; ", unhealthyResults.Select(r => r.Description));
                var data = new Dictionary<string, object>();

                foreach (var result in unhealthyResults) {
                    if (result.Data != null) {
                        foreach (var kvp in result.Data) {
                            data[$"dependency_{kvp.Key}"] = kvp.Value;
                        }
                    }
                }

                return HealthCheckResult.Unhealthy(
                    description: $"Dependencies issues: {errors}",
                    data: data
                );
            }

            return HealthCheckResult.Healthy();
        }
    }
}
