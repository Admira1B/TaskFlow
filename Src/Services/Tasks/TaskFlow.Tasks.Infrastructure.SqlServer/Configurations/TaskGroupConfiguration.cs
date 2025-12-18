using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Configurations {
    public class TaskGroupConfiguration : IEntityTypeConfiguration<TaskGroup> {
        public void Configure(EntityTypeBuilder<TaskGroup> builder) {
            builder.ToTable("TaskGroups", "Tasks");
            
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(g => g.ProjectId);
            
            builder.HasIndex(g => new { g.ProjectId, g.Name })
                .IsUnique();

            builder.HasMany(g => g.TaskItems)
                .WithOne(t => t.Group)
                .HasForeignKey(t => t.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
