using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Domain.Contracts.Repositories {
    public interface IUserRepository {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
        Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default);
    }
}
