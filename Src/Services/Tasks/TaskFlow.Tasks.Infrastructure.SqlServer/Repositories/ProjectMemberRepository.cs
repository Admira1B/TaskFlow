using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Repositories {
    public class ProjectMemberRepository(TasksServiceDbContext dbContext) : IProjectMemberRepository {
        private readonly TasksServiceDbContext _dbContext = dbContext;

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) { 
            return await _dbContext.Members
                .AsNoTracking()
                .AnyAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<bool> UserExistsInProjectAsync(Guid userId, Guid projectId, CancellationToken cancellationToken = default) {
            return await _dbContext.Members
                .AsNoTracking()
                .AnyAsync(m => m.UserId == userId && (m.ProjectId == projectId), cancellationToken);
        }

        public async Task<ProjectMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<ProjectMember>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ProjectMember>> GetByProjectAsync(Guid projectId, CancellationToken cancellationToken = default) {
            return await _dbContext.Members
                .AsNoTracking()
                .Where(m => m.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ProjectMember member, CancellationToken cancellationToken = default) {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
            await _dbContext.Members
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task UpdateAsync(ProjectMember member, CancellationToken cancellationToken = default) {
            var existing = await _dbContext.Members
                .FirstAsync(m => m.Id == member.Id, cancellationToken);

            existing.Role = member.Role;
            existing.MarkAsUpdated();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
