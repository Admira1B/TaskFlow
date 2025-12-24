using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskItemRepository(TaskServiceDbContext dbContext) : ITaskItemRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskItem?> GetByIdAsync(Guid id) {
            return await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TaskItem>> GetByReporterAsync(Guid userId) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.ReporterId == userId)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetByAssigneeAsync(Guid userId) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedId == userId)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetByProjectAsync(Guid projectId) {
            var groupIds = await _dbContext.Groups
                .AsNoTracking()
                .Where(g => g.ProjectId == projectId)
                .Select(g => g.Id)
                .ToListAsync();

            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => groupIds.Contains(t.GroupId))
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetByTaskGroupAsync(Guid groupId) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.GroupId == groupId)
                .ToListAsync();
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
            existing.Priority = task.Priority;
            existing.GroupId = task.GroupId;

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
