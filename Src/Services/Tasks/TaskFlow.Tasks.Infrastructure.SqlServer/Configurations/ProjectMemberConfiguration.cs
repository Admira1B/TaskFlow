using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Configurations {
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember> {
        public void Configure(EntityTypeBuilder<ProjectMember> builder) {
            builder.ToTable("ProjectMembers", "Tasks");
            
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Role)
                .HasConversion<int>()
                .HasDefaultValue(ProjectRole.Member);

            builder.HasIndex(m => m.ProjectId);

            builder.HasIndex(m => m.UserId);
            
            builder.HasIndex(m => new { m.ProjectId, m.UserId })
                .IsUnique();
        }
    }
}
