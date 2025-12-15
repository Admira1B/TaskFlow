using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Repositories {
    public class RoleRepository(IdentityDbContext dbContext) : IRoleRepository {
        private readonly IdentityDbContext _dbContext = dbContext;
        public async Task<IEnumerable<Role>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
            return await _dbContext.Roles
                         .AsNoTracking()
                         .OrderBy(r => r.Name)
                         .Skip(pageSize * (page - 1))
                         .Take(pageSize)
                         .ToListAsync(cancellationToken);
        }
    }
}
