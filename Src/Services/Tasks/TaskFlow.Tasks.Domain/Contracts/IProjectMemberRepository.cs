using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectMemberRepository {
        Task<ProjectMember?> GetByIdAsync(Guid id);
        Task<List<ProjectMember>> GetByUserAsync(Guid userId);
        Task<List<ProjectMember>> GetByProjectAsync(Guid projectId);
        Task AddAsync(ProjectMember member);
        Task UpdateAsync(ProjectMember member);
        Task DeleteAsync(Guid id);
    }
}
