using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskGroupRepository(TaskServiceDbContext dbContext) : ITaskGroupRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskGroup?> GetByIdAsync(Guid id) {
            return await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        
        public async Task<List<TaskGroup>> GetByProjectAsync(Guid projectId) {
            return await _dbContext.Groups
                .AsNoTracking()
                .Where(g => g.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<TaskGroup?> GetByIdWithTasksAsync(Guid id) {
            return await _dbContext.Groups
                .AsNoTracking()
                .Include(g => g.TaskItems)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(TaskGroup group) {
            await _dbContext.Groups
                .AddAsync(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskGroup group) {
            var existing = await _dbContext.Groups
                .FirstAsync(g => g.Id == group.Id);

            existing.Name = group.Name;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            await _dbContext.Groups
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
