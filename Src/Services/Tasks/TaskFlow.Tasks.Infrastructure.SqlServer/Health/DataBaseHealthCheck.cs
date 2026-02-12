using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Health {
    public class DataBaseHealthCheck(TaskServiceDbContext dbContext) : IHealthCheck {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

                if (!canConnect) {
                    return HealthCheckResult.Unhealthy(
                        description: "Cannot connect to SQL Server database",
                        data: new Dictionary<string, object> {
                            ["database"] = _dbContext.Database.GetDbConnection().Database,
                            ["server"] = _dbContext.Database.GetDbConnection().DataSource,
                            ["connection_state"] = _dbContext.Database.GetDbConnection().State.ToString()
                        }
                    );
                }

                return HealthCheckResult.Healthy();
            } catch (Exception ex) {
                return HealthCheckResult.Unhealthy(
                    description: $"Database health check failed: {ex.Message}",
                    exception: ex,
                    data: new Dictionary<string, object> {
                        ["database_error"] = ex.GetType().Name,
                    }
                );
            }
        }
    }
}
