using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITenantRepository
    {
        Task<IEnumerable<Tenant>> GetAllTenantsAsync(bool trackChanges);
        Task<Tenant> GetTenantAsync(Guid tenantId, bool trackChanges);
    }
}
