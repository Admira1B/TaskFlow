using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Configurations {
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem> {
        public void Configure(EntityTypeBuilder<TaskItem> builder) {
            builder.ToTable("TaskItems", "Tasks");
            
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(4000);

            builder.Property(t => t.Priority)
                .HasConversion<int>()
                .HasDefaultValue(Priority.Medium);

            builder.HasIndex(t => t.GroupId);
            
            builder.HasIndex(t => t.ReporterId);
            
            builder.HasIndex(t => t.AssignedId);
            
            builder.HasIndex(t => new { t.GroupId, t.Priority });

            builder.HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
