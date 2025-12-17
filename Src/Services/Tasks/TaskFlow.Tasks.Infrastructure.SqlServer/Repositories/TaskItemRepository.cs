using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskItemRepository(TaskServiceDbContext dbContext) : ITaskItemRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskItem?> GetByIdAsync(Guid id) {
            return await _dbContext.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TaskItem> AddAsync(TaskItem task) {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<bool> UpdateAsync(TaskItem task) {
            var existing = await _dbContext.Tasks.Where(t => t.Id == task.Id).FirstOrDefaultAsync();

            if (existing is null) {
                return false;
            }

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.AssignedId = task.AssignedId;

            existing.MarkAsUpdated();

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id) {
            var rows = await _dbContext.Tasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();

            return rows > 0;
        }
    }
}
