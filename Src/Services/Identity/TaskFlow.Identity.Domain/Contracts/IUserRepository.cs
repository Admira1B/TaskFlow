using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Domain.Contracts {
    public interface IUserRepository {
        Task<IEnumerable<User>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
