using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectMemberRepository(TaskServiceDbContext dbContext) : IProjectMemberRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<ProjectMember?> GetByIdAsync(Guid id) {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<ProjectMember>> GetByUserAsync(Guid userId) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ProjectMember>> GetByProjectAsync(Guid projectId) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectMember member) {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            await _dbContext.Members
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task UpdateAsync(ProjectMember member) {
            var existing = await _dbContext.Members
                .FirstAsync(m => m.Id == member.Id);

            existing.Role = member.Role;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }
    }
}
