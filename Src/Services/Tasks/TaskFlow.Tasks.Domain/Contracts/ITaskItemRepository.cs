using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ITaskItemRepository {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByReporterAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByAssigneeAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<List<TaskItem>> GetByTaskGroupAsync(Guid groupId, CancellationToken cancellationToken = default);
        Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
