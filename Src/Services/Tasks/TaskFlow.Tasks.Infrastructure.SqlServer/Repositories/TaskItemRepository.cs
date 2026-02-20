using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskItemRepository(TasksServiceDbContext dbContext) : ITaskItemRepository {
        private readonly TasksServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<List<TaskItem>> GetByReporterAsync(Guid userId, CancellationToken cancellationToken = default) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.ReporterId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskItem>> GetByAssigneeAsync(Guid userId, CancellationToken cancellationToken = default) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskItem>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default) {
            var groupIds = await _dbContext.Groups
                .AsNoTracking()
                .Where(g => g.ProjectId == projectId)
                .Select(g => g.Id)
                .ToListAsync(cancellationToken);

            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => groupIds.Contains(t.GroupId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TaskItem>> GetByTaskGroupAsync(Guid groupId, CancellationToken cancellationToken = default) {
            return await _dbContext.Tasks
                .AsNoTracking()
                .Where(t => t.GroupId == groupId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default) {
            await _dbContext.Tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default) {
            var existing = await _dbContext.Tasks
                .Where(t => t.Id == task.Id)
                .FirstAsync(cancellationToken);

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.AssignedId = task.AssignedId;
            existing.Priority = task.Priority;
            existing.GroupId = task.GroupId;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            await _dbContext.Tasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
