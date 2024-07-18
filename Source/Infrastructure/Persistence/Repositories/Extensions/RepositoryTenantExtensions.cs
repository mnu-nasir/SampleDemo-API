using Entities.Entities;

namespace Persistence.Repositories.Extensions
{
    public static class RepositoryTenantExtensions
    {
        public static IQueryable<Tenant> FilterTenants(this IQueryable<Tenant> tenants, bool? isActive)
        {
            if (!isActive.HasValue)
                return tenants;

            return tenants.Where(t => t.IsActive == isActive);
        }

        public static IQueryable<Tenant> Search(this IQueryable<Tenant> tenants, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return tenants;

            return tenants.Where(t => t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}
