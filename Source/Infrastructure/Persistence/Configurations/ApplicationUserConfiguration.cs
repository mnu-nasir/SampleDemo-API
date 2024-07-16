using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Table Name
            builder.ToTable(nameof(ApplicationUser));

            // Key
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnOrder(1);
            builder.Property(e => e.FirstName).IsUnicode(true).HasMaxLength(50).HasColumnOrder(2);
            builder.Property(e => e.LastName).IsUnicode(true).HasMaxLength(50).HasColumnOrder(3);
            builder.Property(e => e.DisplayName).IsUnicode(true).HasMaxLength(50).HasColumnOrder(4);

            builder.Property(e => e.UserName).IsRequired().HasColumnOrder(5);
            builder.Property(e => e.NormalizedUserName).HasColumnOrder(6);
            builder.Property(e => e.Email).IsRequired().IsUnicode(false).HasColumnOrder(7);
            builder.Property(e => e.NormalizedEmail).IsUnicode(false).HasColumnOrder(8);
            builder.Property(e => e.EmailConfirmed).IsRequired().HasColumnOrder(9);
            builder.Property(e => e.PasswordHash).HasColumnOrder(10);
            builder.Property(e => e.PhoneNumber).HasMaxLength(20).HasColumnOrder(11);
            builder.Property(e => e.PhoneNumberConfirmed).HasColumnOrder(12);
            builder.Property(e => e.TwoFactorEnabled).HasColumnOrder(13);
            builder.Property(e => e.LockoutEnd).HasColumnOrder(14);
            builder.Property(e => e.LockoutEnabled).HasColumnOrder(15);
            builder.Property(e => e.AccessFailedCount).HasColumnOrder(16);

            builder.Property(e => e.IsActive).HasColumnOrder(17);
            builder.Property(e => e.LastLogin).HasColumnOrder(18);

            builder.Property(x => x.TenantId).HasColumnOrder(19);
            builder.Property(x => x.CreatedBy).IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnOrder(20);
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnOrder(21);
            builder.Property(x => x.ModifiedBy).IsUnicode(false).HasMaxLength(50).HasColumnOrder(22);
            builder.Property(e => e.ModifiedAt).HasColumnOrder(23);
            builder.Property(e => e.IsDeleted).HasColumnOrder(24);
            builder.Property(x => x.DeletedBy).IsUnicode(false).HasMaxLength(50).HasColumnOrder(25);
            builder.Property(x => x.DeletedAt).HasColumnOrder(26);

            // Index
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.UserName).IsUnique();

            // Ignore
            builder.Ignore(e => e.ConcurrencyStamp);
            builder.Ignore(e => e.SecurityStamp);

            // Query Filter
            builder.HasQueryFilter(x => x.IsDeleted == false);

            // Relationships
            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.ApplicationUsers)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
