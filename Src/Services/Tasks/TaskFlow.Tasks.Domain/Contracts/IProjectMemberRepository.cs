using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectMemberRepository {
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UserExistsInProjectAsync(Guid userId, Guid projectId, CancellationToken cancellationToken = default);
        Task<ProjectMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ProjectMember>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<List<ProjectMember>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(ProjectMember member, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProjectMember member, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
