using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectRepository {
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project?> GetByIdWithGroupsAsync(Guid id);
        Task<Project> AddAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> IsMemberOfProjectAsync(Guid projectId, Guid userId);
    }
}
