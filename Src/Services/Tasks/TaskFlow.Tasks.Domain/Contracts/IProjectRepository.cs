using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface IProjectRepository {
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project?> GetByIdWithGroupsAsync(Guid id);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Guid id);
        Task<bool> IsMemberOfProjectAsync(Guid projectId, Guid userId);
    }
}
