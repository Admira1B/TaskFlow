using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Infrastructure.SqlServer.Configurations {
    internal class UserConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("Users", "Identity");

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.NormalizedUserName).IsUnique();

            builder.Property(u => u.FirstName)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);
        }
    }
}
