using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Core.Health;
using TaskFlow.Shared.Core.Abstractions;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Shared.Messaging.RabbitMQ.Health;

namespace TaskFlow.Tasks.API.Health {
    public class TasksServiceHealthCheck(
        Shared.Core.Interfaces.ILogger logger, 
        ConsulHealthCheck consulHealth,
        RabbitMqHealthCheck rabbitMqHealth, 
        DataBaseHealthCheck<TasksServiceDbContext> dataBaseHealth) 
        : ServiceHealthCheckBase(logger, "Tasks Service") {
        private readonly ConsulHealthCheck _consulHealth = consulHealth;
        private readonly RabbitMqHealthCheck _rabbitMqHealth = rabbitMqHealth;
        private readonly DataBaseHealthCheck<TasksServiceDbContext> _dataBaseHealth = dataBaseHealth;

        protected override async Task<HealthCheckResult> CheckDependenciesHealthAsync(HealthCheckContext context, CancellationToken cancellationToken) {
            List<HealthCheckResult> results = [];

            results.Add(await _consulHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _rabbitMqHealth.CheckHealthAsync(context, cancellationToken));
            results.Add(await _dataBaseHealth.CheckHealthAsync(context, cancellationToken));

            return GetDependenciesHealthCheckResult(results);
        }
    }
}
