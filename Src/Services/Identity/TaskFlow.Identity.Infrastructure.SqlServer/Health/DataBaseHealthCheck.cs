using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Health {
    public class DataBaseHealthCheck(IdentityDbContext dbContext) : IHealthCheck {
        private readonly IdentityDbContext _dbContext = dbContext;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default) {
            try {
                var canConnect = await _dbContext.Database.CanConnectAsync(ct);

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
