using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ITaskItemRepository {
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<List<TaskItem>> GetByReporterAsync(Guid userId);
        Task<List<TaskItem>> GetByAssigneeAsync(Guid userId);
        Task<List<TaskItem>> GetByProjectAsync(Guid projectId);
        Task<List<TaskItem>> GetByTaskGroupAsync(Guid groupId);
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(Guid id);
    }
}
