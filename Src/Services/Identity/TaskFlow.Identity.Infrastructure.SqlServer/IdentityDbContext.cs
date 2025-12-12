using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Infrastructure.SqlServer {
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
               : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options) {
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}
