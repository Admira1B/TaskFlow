using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectRepository(TaskServiceDbContext dbContext) : IProjectRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<Project?> GetByIdAsync(Guid id) {
            return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project?> GetByIdWithGroupsAsync(Guid id) {
            return await _dbContext.Projects
                .Include(p => p.Groups)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddAsync(Project project) {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<bool> UpdateAsync(Project project) {
            var existing = await _dbContext.Projects.FindAsync(project.Id);
            
            if (existing is null) { 
                return false; 
            }

            existing.Name = project.Name;
            existing.Description = project.Description;
            existing.IsActive = project.IsActive;

            existing.MarkAsUpdated();

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id) {
            var rows = await _dbContext.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            return rows > 0;
        }

        public async Task<bool> IsMemberOfProjectAsync(Guid projectId, Guid userId) {
            return await _dbContext.Members
                .AnyAsync(m => m.ProjectId == projectId && m.UserId == userId);
        }
    }
}
