using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class TaskGroupRepository(TaskServiceDbContext dbContext) : ITaskGroupRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;
        
        public async Task<TaskGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Groups
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }
        
        public async Task<List<TaskGroup>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default) {
            return await _dbContext.Groups
                .AsNoTracking()
                .Where(g => g.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TaskGroup group, CancellationToken cancellationToken = default) {
            await _dbContext.Groups
                .AddAsync(group);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TaskGroup group, CancellationToken cancellationToken = default) {
            var existing = await _dbContext.Groups
                .FirstAsync(g => g.Id == group.Id, cancellationToken);

            existing.Name = group.Name;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            await _dbContext.Groups
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
