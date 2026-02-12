using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ITaskGroupRepository {
        Task<TaskGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TaskGroup>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task AddAsync(TaskGroup group, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskGroup group, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
