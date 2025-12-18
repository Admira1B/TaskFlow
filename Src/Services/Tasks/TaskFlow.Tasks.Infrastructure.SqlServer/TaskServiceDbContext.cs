using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Infrastructure.SqlServer.Configurations;

namespace TaskFlow.Tasks.Infrastructure.SqlServer {
    public class TaskServiceDbContext(DbContextOptions<TaskServiceDbContext> options) : DbContext(options) {
        public DbSet<ProjectMember> Members { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskGroup> Groups { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.HasDefaultSchema("Tasks");

            builder.ApplyConfiguration(new ProjectMemberConfiguration());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new TaskGroupConfiguration());
            builder.ApplyConfiguration(new TaskItemConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
