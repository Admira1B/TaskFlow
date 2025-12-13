using TaskFlow.Identity.Domain.Contracts.Repositories;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Repositories {
    public class UserRepository : IUserRepository {
        public Task AddAsync(User user, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }
    }
}
