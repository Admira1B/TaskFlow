using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Shared.Messaging.Health;
using TaskFlow.Tasks.Infrastructure.SqlServer.Health;
using TaskFlow.Shared.Core.Abstractions;

namespace TaskFlow.Tasks.API.Health {
    public class TasksServiceHealthCheck(Shared.Core.Interfaces.ILogger logger, DataBaseHealthCheck dataBaseHealth, RabbitMqHealthCheck rabbitMqHealth, ConsulHealthCheck consulHealth) :
             ServiceHealthCheckBase(logger, "Tasks Service") {
        private readonly DataBaseHealthCheck _dataBaseHealth = dataBaseHealth;
        private readonly RabbitMqHealthCheck _rabbitMqHealth = rabbitMqHealth;
        private readonly ConsulHealthCheck _consulHealth = consulHealth;

        protected override async Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            var checks = new List<Task<HealthCheckResult>> {
                _dataBaseHealth.CheckHealthAsync(context, cancellationToken),
                _rabbitMqHealth.CheckHealthAsync(context, cancellationToken),
                _consulHealth.CheckHealthAsync(context, cancellationToken)
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
