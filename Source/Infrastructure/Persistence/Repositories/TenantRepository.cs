using Contracts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;

namespace Persistence.Repositories
{
    internal sealed class TenantRepository : RepositoryBase<TenantContext, Tenant>, ITenantRepository
    {
        public TenantRepository(TenantContext tenantContext)
            : base(tenantContext)
        {
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsAsync(bool trackChanges)
        {
            var tenants = await FindAll(trackChanges)
                                    .OrderBy(x => x.Title)
                                    .ToListAsync();
            return tenants;
        }

        public async Task<Tenant> GetTenantAsync(Guid tenantId, bool trackChanges)
        {
            var tenant = await FindByCondition(t => t.Id.Equals(tenantId), trackChanges)
                .SingleOrDefaultAsync();
            return tenant;
        }
    }
}
