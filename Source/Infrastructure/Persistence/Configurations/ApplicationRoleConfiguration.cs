﻿using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Table Name
        builder.ToTable(nameof(ApplicationRole));

        // Key
        builder.HasKey(x => x.Id);

        // Property
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsUnicode(false)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        // Ignore
        builder.Ignore(x => x.ConcurrencyStamp);

        // Query Filter
        builder.HasQueryFilter(t => t.IsDeleted == false);
    }
}
