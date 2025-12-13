using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Configurations {
    internal class RoleConfiguration : IEntityTypeConfiguration<Role> {
        public void Configure(EntityTypeBuilder<Role> builder) {
            builder.ToTable("Roles", "Identity");

            builder.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.UpdatedAt)
                .IsRequired(false);
        }
    }
}
