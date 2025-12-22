using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Domain.Contracts {
    public interface ICommentRepository {
        Task<Comment?> GetByIdAsync(Guid id);
        Task<List<Comment>> GetByTaskIdAsync(Guid taskId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Guid id);
    }
}
