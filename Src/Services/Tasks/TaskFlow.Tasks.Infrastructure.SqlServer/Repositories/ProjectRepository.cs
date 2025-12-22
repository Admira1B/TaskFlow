using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectRepository(TaskServiceDbContext dbContext) : IProjectRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<Project?> GetByIdAsync(Guid id) {
            return await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project?> GetByIdWithGroupsAsync(Guid id) {
            return await _dbContext.Projects
                .Include(p => p.Groups)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Project project) {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project) {
            var existing = await _dbContext.Projects
                .FirstAsync(p => p.Id == project.Id);

            existing.Name = project.Name;
            existing.Description = project.Description;
            existing.IsActive = project.IsActive;

            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            await _dbContext.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> IsMemberOfProjectAsync(Guid projectId, Guid userId) {
            return await _dbContext.Members
                .AnyAsync(m => m.ProjectId == projectId && m.UserId == userId);
        }
    }
}
