using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Domain.Contracts.Repositories {
    public interface IRoleRepository {
        Task<IEnumerable<Role>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
