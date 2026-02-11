using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectMemberRepository(TaskServiceDbContext dbContext) : IProjectMemberRepository {
        private readonly TaskServiceDbContext _dbContext = dbContext;

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) { 
            return await _dbContext.Members
                .AsNoTracking()
                .AnyAsync(m => m.Id == id);
        }

        public async Task<bool> UserExistsInProjectAsync(Guid userId, Guid projectId, CancellationToken cancellationToken) {
            return await _dbContext.Members
                .AsNoTracking()
                .AnyAsync(m => m.UserId == userId && (m.ProjectId == projectId));
        }

        public async Task<ProjectMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<ProjectMember>> GetByUserAsync(Guid userId, CancellationToken cancellationToken) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ProjectMember>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectMember member, CancellationToken cancellationToken) {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) {
            await _dbContext.Members
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task UpdateAsync(ProjectMember member, CancellationToken cancellationToken) {
            var existing = await _dbContext.Members
                .FirstAsync(m => m.Id == member.Id);

            existing.Role = member.Role;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync();
        }
    }
}
