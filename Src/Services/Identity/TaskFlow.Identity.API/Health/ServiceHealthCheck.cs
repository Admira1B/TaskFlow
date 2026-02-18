using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Messaging.Health;
using TaskFlow.Identity.Application.Health;
using TaskFlow.Identity.Infrastructure.SqlServer.Health;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Shared.Core.Abstractions;

namespace TaskFlow.Identity.API.Health {
    public class ServiceHealthCheck(Shared.Core.Interfaces.ILogger logger, IdentityHealthCheck identityHealth, DataBaseHealthCheck dataBaseHealth, RabbitMqHealthCheck rabbitMqHealth, ConsulHealthCheck consulHealth) :
                 ServiceHealthCheckBase(logger, "Identity Service") {
        private readonly IdentityHealthCheck _identityHealth = identityHealth;
        private readonly DataBaseHealthCheck _dataBaseHealth = dataBaseHealth;
        private readonly RabbitMqHealthCheck _rabbitMqHealth = rabbitMqHealth;
        private readonly ConsulHealthCheck _consulHealth = consulHealth;

        protected override async Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            List<HealthCheckResult> results = [];

            results.Add(await _identityHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _rabbitMqHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _dataBaseHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _consulHealth.CheckHealthAsync(context, cancellationToken));

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
