using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectRepository(TaskServiceDbContext dbContext) : IProjectRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) { 
            return await _dbContext.Projects
                .AsNoTracking()
                .AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Project?> GetByIdWithGroupsAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Groups)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Project>> GetByProjectMemberAsync(Guid userId, CancellationToken cancellationToken = default) {
            return await _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.Members.Any(m => m.UserId == userId)) 
                .Where(p => p.OwnerId != userId) 
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Project>> GetByOwnerAsync(Guid userId, CancellationToken cancellationToken = default) {
            return await _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken = default) {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default) {
            var existing = await _dbContext.Projects
                .FirstAsync(p => p.Id == project.Id, cancellationToken);

            existing.Name = project.Name;
            existing.Description = project.Description;
            existing.IsActive = project.IsActive;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            await _dbContext.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
