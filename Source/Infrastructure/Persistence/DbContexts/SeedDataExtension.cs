using Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DbContexts;

public static class SeedDataExtension
{
    public static void SeedData(this ModelBuilder builder)
    {
        builder.Entity<Tenant>().HasData
        (
            new Tenant
            {
                Id = new Guid("0bfde7b5-5d86-4ab5-9604-e19dc29d9800"),
                Title = "Nasir Inc",
                IsPrimary = true,
                CreatedBy = "",
                CreatedAt = new DateTime(2024, 09, 04, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        builder.Entity<ApplicationRole>().HasData
        (
            new ApplicationRole
            {
                Id = new Guid("e3cc0c53-8409-4a08-928a-e0996bc14e23"),
                Name = "Super Admin",
                NormalizedName = "SUPER ADMIN",
                CreatedBy = "",
                CreatedAt = new DateTime(2024, 09, 04, 0, 0, 0, DateTimeKind.Utc)
            },
            new ApplicationRole
            {
                Id = new Guid("6c894122-fbe3-41b4-9b19-4e3ac41a84b2"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                CreatedBy = "",
                CreatedAt = new DateTime(2024, 09, 04, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        ApplicationUser superAdminUser = new ApplicationUser()
        {
            Id = new Guid("753796ac-9ce4-47d5-a5e3-8a9634f794fc"),
            UserName = "mnu.nasir@gmail.com",
            Email = "mnu.nasir@gmail.com",
            NormalizedEmail = "mnu.nasir@gmail.com".ToUpper(),
            NormalizedUserName = "mnu.nasir@gmail.com".ToUpper(),
            TenantId = new Guid("0bfde7b5-5d86-4ab5-9604-e19dc29d9800"),
            FirstName = "Mohammad",
            LastName = "Nasir Uddin",
            CreatedBy = "753796ac-9ce4-47d5-a5e3-8a9634f794fc",
            CreatedAt = new DateTime(2024, 09, 04, 0, 0, 0, DateTimeKind.Utc),
        };
        var superAdminUserPassword = new PasswordHasher<ApplicationUser>();
        var superAdminUserHashed = superAdminUserPassword.HashPassword(superAdminUser, "nasir123");
        superAdminUser.PasswordHash = superAdminUserHashed;
        builder.Entity<ApplicationUser>().HasData(superAdminUser);

        builder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid>
            {
                RoleId = new Guid("e3cc0c53-8409-4a08-928a-e0996bc14e23"),
                UserId = new Guid("753796ac-9ce4-47d5-a5e3-8a9634f794fc")
            });
    }
}
