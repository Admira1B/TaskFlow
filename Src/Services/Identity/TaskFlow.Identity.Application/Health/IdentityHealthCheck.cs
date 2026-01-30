using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Application.Health {
    public class IdentityHealthCheck(UserManager<User> manager) : IHealthCheck {
        private readonly UserManager<User> _manager = manager;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default) {
            try {
                var usersCount = await _manager.Users.CountAsync(ct);

                return HealthCheckResult.Healthy();
            } catch (Exception ex) {
                return HealthCheckResult.Unhealthy(
                    description: $"Identity framework health check failed: {ex.Message}",
                    exception: ex
                );
            }
        }
    }
}
