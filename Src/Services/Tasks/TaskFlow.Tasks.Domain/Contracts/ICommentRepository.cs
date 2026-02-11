using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ICommentRepository {
        Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken);
        Task AddAsync(Comment comment, CancellationToken cancellationToken);
        Task UpdateAsync(Comment comment, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
