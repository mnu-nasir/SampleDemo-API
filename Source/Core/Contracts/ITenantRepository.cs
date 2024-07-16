using Entities.Entities;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface ITenantRepository
    {
        Task<PagedList<Tenant>> GetAllTenantsAsync(TenantParameters tenantParameters, bool trackChanges);
        Task<Tenant> GetTenantAsync(Guid tenantId, bool trackChanges);
        void CreateTenant(Tenant tenant);
        Task<IEnumerable<Tenant>> GetTenantsByIdAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteTenant(Tenant tenant);
    }
}
