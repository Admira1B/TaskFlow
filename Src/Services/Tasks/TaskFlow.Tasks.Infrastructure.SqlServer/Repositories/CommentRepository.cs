using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class CommentRepository(TaskServiceDbContext dbContext) : ICommentRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<List<Comment>> GetByTaskIdAsync(Guid taskId) {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id) {
            return await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Comment comment) {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            await _dbContext.Comments
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task UpdateAsync(Comment comment) {
            var existing = await _dbContext.Comments
                .FirstAsync(c => c.Id == comment.Id);

            existing.Content = comment.Content;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }
    }
}
