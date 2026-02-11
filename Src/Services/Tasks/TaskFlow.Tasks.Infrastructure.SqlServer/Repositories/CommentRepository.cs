using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class CommentRepository(TaskServiceDbContext dbContext) : ICommentRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken) {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Comment comment, CancellationToken cancellationToken) {
            await _dbContext.Comments.AddAsync(comment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) {
            await _dbContext.Comments
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task UpdateAsync(Comment comment, CancellationToken cancellationToken) {
            var existing = await _dbContext.Comments
                .FirstAsync(c => c.Id == comment.Id, cancellationToken);

            existing.Content = comment.Content;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
