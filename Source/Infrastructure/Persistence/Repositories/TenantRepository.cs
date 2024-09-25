using Contracts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Persistence.Repositories.Extensions;
using Shared.RequestFeatures;

namespace Persistence.Repositories;

internal sealed class TenantRepository : RepositoryBase<TenantContext, Tenant>, ITenantRepository
{
    public TenantRepository(TenantContext tenantContext)
        : base(tenantContext)
    {
    }

    public async Task<PagedList<Tenant>> GetAllTenantsAsync(TenantParameters tenantParameters, bool trackChanges)
    {
        var tenants = await FindAll(trackChanges)
            .FilterTenants(tenantParameters.IsActive)
            .Search(tenantParameters.SearchTerm)
            .Sort(tenantParameters.OrderBy)
            .OrderBy(t => t.Title)
            .Skip((tenantParameters.PageNumber - 1) * tenantParameters.PageSize)
            .Take(tenantParameters.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<Tenant>(tenants, count, tenantParameters.PageNumber, tenantParameters.PageSize);
    }

    public async Task<Tenant> GetTenantAsync(Guid tenantId, bool trackChanges)
    {
        var tenant = await FindByCondition(t => t.Id.Equals(tenantId), trackChanges)
            .SingleOrDefaultAsync();
        return tenant;
    }

    public void CreateTenant(Tenant tenant)
    {
        Create(tenant);
    }

    public async Task<IEnumerable<Tenant>> GetTenantsByIdAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        var tenants = await FindByCondition(t => ids.Contains(t.Id), trackChanges).ToListAsync();
        return tenants;
    }

    public void DeleteTenant(Tenant tenant)
    {
        Delete(tenant);
    }
}
