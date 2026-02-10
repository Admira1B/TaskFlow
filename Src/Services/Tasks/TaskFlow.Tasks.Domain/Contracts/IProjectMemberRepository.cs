using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectMemberRepository {
        Task<bool> ExistsAsync(Guid id);
        Task<bool> UserExistsInProjectAsync(Guid userId, Guid projectId);
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<List<ProjectMember>> GetByProjectAsync(Guid projectId);
        Task<List<ProjectMember>> GetByUserAsync(Guid userId);
        Task AddAsync(ProjectMember member);
        Task UpdateAsync(ProjectMember member);
        Task DeleteAsync(Guid id);
    }
}
