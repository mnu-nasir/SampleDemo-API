using Entities.Entities;
using Persistence.Repositories.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Persistence.Repositories.Extensions;

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

    public static IQueryable<Tenant> Sort(this IQueryable<Tenant> tenants, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return tenants.OrderBy(e => e.Title);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Tenant>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return tenants.OrderBy(e => e.Title);

        return tenants.OrderBy(orderQuery);
    }
}
