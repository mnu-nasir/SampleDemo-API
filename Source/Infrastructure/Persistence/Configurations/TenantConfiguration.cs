using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // Table Name
            builder.ToTable(nameof(Tenant));

            // Key
            builder.HasKey(t => t.Id);

            // Configurations
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Title).IsUnicode(true).HasMaxLength(100);
            builder.Property(t => t.Address).IsUnicode(true).HasMaxLength(150);

            builder.Property(x => x.CreatedBy).IsUnicode(false).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.ModifiedBy).IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.DeletedBy).IsUnicode(false).HasMaxLength(50);

            // Query Filter
            builder.HasQueryFilter(t => t.IsDeleted == false);

            // Relationships
            builder.HasMany(t => t.Employees)
                .WithOne(t => t.Tenant)
                .HasForeignKey(t => t.TenantId);

            builder.HasMany(t => t.ApplicationUsers)
                .WithOne(t => t.Tenant)
                .HasForeignKey(t => t.TenantId)
                .IsRequired();
        }
    }
}
