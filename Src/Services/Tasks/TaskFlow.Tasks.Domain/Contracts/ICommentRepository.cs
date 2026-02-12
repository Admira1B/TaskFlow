using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ICommentRepository {
        Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
        Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
        Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
