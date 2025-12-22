using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ITaskGroupRepository {
        Task<TaskGroup?> GetByIdAsync(Guid id);
        Task<TaskGroup?> GetByIdWithTasksAsync(Guid id);
        Task<List<TaskGroup>> GetByProjectAsync(Guid projectId);
        Task AddAsync(TaskGroup group);
        Task UpdateAsync(TaskGroup group);
        Task DeleteAsync(Guid id);
    }
}
