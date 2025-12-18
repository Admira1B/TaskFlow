using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Repositories {
    public class UserRepository(IdentityServiceDbContext dbContext) : IUserRepository {
        private readonly IdentityServiceDbContext _dbContext = dbContext;

        public async Task<IEnumerable<User>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
            return await _dbContext.Users
                         .AsNoTracking()
                         .OrderBy(u => u.UserName)
                         .Skip(pageSize * (page - 1))
                         .Take(pageSize)
                         .ToListAsync(cancellationToken);

        }
    }
}
