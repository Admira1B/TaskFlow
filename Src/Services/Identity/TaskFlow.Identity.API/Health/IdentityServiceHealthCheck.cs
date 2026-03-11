using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Core.Health;
using TaskFlow.Shared.Core.Abstractions;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Identity.Application.Health;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Shared.Messaging.RabbitMQ.Health;

namespace TaskFlow.Identity.API.Health {
    public class IdentityServiceHealthCheck(
        Shared.Core.Interfaces.ILogger logger,
        ConsulHealthCheck consulHealth,
        RabbitMqHealthCheck rabbitMqHealth, 
        IdentityHealthCheck identityHealth, 
        DataBaseHealthCheck<IdentityServiceDbContext> dataBaseHealth) 
        : ServiceHealthCheckBase(logger, "Identity Service") {
        private readonly ConsulHealthCheck _consulHealth = consulHealth;
        private readonly RabbitMqHealthCheck _rabbitMqHealth = rabbitMqHealth;
        private readonly IdentityHealthCheck _identityHealth = identityHealth;
        private readonly DataBaseHealthCheck<IdentityServiceDbContext> _dataBaseHealth = dataBaseHealth;

        protected override async Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            List<HealthCheckResult> results = [];

            results.Add(await _consulHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _rabbitMqHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _identityHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _dataBaseHealth.CheckHealthAsync(context, cancellationToken));

            return GetDependenciesHealthCheckResult(results);
        }
    }
}
