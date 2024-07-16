using Entities.Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Table Name
            builder.ToTable(nameof(Employee));

            // Key
            builder.HasKey(e => e.Id);

            // Configurations
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).IsUnicode(true).HasMaxLength(50);
            builder.Property(e => e.LastName).IsUnicode(true).HasMaxLength(50);
            builder.Property(e => e.Email).IsUnicode(false).HasMaxLength(100);
            builder.Property(e => e.MobileNo).IsUnicode(true).HasMaxLength(15);

            builder.Property(e => e.BloodGroup)
                .HasMaxLength(10)
                .HasConversion(b => b.ToString(), g => (BloodGroup)Enum.Parse(typeof(BloodGroup), g))
                .IsUnicode(false);
            
            builder.Property(x => x.CreatedBy).IsUnicode(false).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.ModifiedBy).IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.DeletedBy).IsUnicode(false).HasMaxLength(50);

            // Query Filter
            builder.HasQueryFilter(e => e.IsDeleted == false);

            // Relationships
            builder.HasOne(e => e.Tenant)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
