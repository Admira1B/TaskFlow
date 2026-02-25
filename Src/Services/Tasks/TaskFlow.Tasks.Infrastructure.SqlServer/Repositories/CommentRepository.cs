using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class CommentRepository(TasksServiceDbContext dbContext) : ICommentRepository {
        private readonly TasksServiceDbContext _dbContext = dbContext;

        public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default) {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default) {
            await _dbContext.Comments.AddAsync(comment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            await _dbContext.Comments
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default) {
            var existing = await _dbContext.Comments
                .FirstAsync(c => c.Id == comment.Id, cancellationToken);

            existing.Content = comment.Content;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
