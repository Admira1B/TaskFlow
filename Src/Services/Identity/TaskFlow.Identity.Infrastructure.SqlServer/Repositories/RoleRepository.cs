using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Repositories {
    public class RoleRepository(IdentityServiceDbContext dbContext) : IRoleRepository {
        private readonly IdentityServiceDbContext _dbContext = dbContext;
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
