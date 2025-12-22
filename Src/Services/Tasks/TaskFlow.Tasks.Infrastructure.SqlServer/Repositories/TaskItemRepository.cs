using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskItemRepository(TaskServiceDbContext dbContext) : ITaskItemRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskItem?> GetByIdAsync(Guid id) {
            return await _dbContext.Tasks.Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(TaskItem task) {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem task) {
            var existing = await _dbContext.Tasks
                .Where(t => t.Id == task.Id)
                .FirstAsync();

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.AssignedId = task.AssignedId;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            await _dbContext.Tasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
