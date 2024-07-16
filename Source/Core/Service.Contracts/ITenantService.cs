using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
    public interface ITenantService
    {
        Task<(IEnumerable<TenantDto> tenants, MetaData metaData)> GetAllTenantsAsync(TenantParameters tenantParameters, 
            bool trackChanges);
        Task<TenantDto> GetTenantAsync(Guid tenantId, bool trackChanges);
        Task<TenantDto> CreateTenantAsync(TenantForCreationDto tenant);
        Task<IEnumerable<TenantDto>> GetTenantsByIdAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<TenantDto> tenants, string ids)> CreateTenantCollection(IEnumerable<TenantForCreationDto> tenants);
        Task DeleteTenantAsync(Guid tenantId, bool trackChanges);
        Task UpdateTenantAsync(Guid tenantId, TenantForUpdateDto tenantForUpdate, bool trackChanges);
    }
}
