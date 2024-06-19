using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Table Name
            builder.ToTable(nameof(ApplicationRole));

            // Key
            builder.HasKey(x => x.Id);

            // Property
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnOrder(1);
            builder.Property(x => x.Name).IsRequired().HasColumnOrder(2);
            builder.Property(x => x.NormalizedName).HasColumnOrder(3);

            builder.Property(x => x.CreatedBy)
                .IsUnicode(false)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnOrder(4);
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnOrder(5);
            builder.Property(x => x.IsDeleted).HasColumnOrder(6);
            builder.Property(x => x.DeletedAt).HasColumnOrder(7);

            // Ignore
            builder.Ignore(x => x.ConcurrencyStamp);

            // Query Filter
            builder.HasQueryFilter(t => t.IsDeleted == false);
        }
    }
}
