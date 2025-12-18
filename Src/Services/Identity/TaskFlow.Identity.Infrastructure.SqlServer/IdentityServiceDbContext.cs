using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Infrastructure.SqlServer.Configurations;

namespace TaskFlow.Identity.Infrastructure.SqlServer {
    public class IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
               : IdentityDbContext<User, Role, Guid>(options) {
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.HasDefaultSchema("Identity");

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());

            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "Identity");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "Identity");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "Identity");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "Identity");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "Identity");
        }
    }
}
