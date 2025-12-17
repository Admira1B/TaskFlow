using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Infrastructure.SqlServer.Configurations {
    public class ProjectConfiguration : IEntityTypeConfiguration<Project> {
        public void Configure(EntityTypeBuilder<Project> builder) {
            builder.ToTable("Projects", "Tasks");
            
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(p => p.Description)
                .HasMaxLength(2000);
            
            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(p => p.OwnerId);
            
            builder.HasIndex(p => p.IsActive);

            builder.HasMany(p => p.Groups)
                .WithOne(g => g.Project)
                .HasForeignKey(g => g.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Members)
                .WithOne(m => m.Project)
                .HasForeignKey(m => m.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
