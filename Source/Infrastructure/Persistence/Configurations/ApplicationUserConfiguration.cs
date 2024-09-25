using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Table Name
        builder.ToTable(nameof(ApplicationUser));

        // Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.FirstName).IsUnicode(true).HasMaxLength(50);
        builder.Property(e => e.LastName).IsUnicode(true).HasMaxLength(50);
        builder.Property(e => e.DisplayName).IsUnicode(true).HasMaxLength(50);

        builder.Property(x => x.CreatedBy).IsUnicode(false)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ModifiedBy).IsUnicode(false).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).IsUnicode(false).HasMaxLength(50);

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
