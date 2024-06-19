using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantDto>> GetAllTenantsAsync(bool trackChanges);
        Task<TenantDto> GetTenantAsync(Guid tenantId, bool trackChanges);
    }
}
