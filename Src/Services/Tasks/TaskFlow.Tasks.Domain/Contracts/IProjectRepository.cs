using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectRepository {
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Project?> GetByIdWithGroupsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Project>> GetByProjectMemberAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<Project>> GetByOwnerAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(Project project, CancellationToken cancellationToken = default);
        Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
