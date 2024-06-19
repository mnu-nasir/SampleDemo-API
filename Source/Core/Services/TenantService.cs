using Contracts;
using Entities.Entities;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal sealed class TenantService : ITenantService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public TenantService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<TenantDto>> GetAllTenantsAsync(bool trackChanges)
        {
            var tenants = await _repository.Tenant.GetAllTenantsAsync(trackChanges);
            return tenants.Select(x => new TenantDto
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address
            }).ToList();
        }

        public async Task<TenantDto> GetTenantAsync(Guid tenantId, bool trackChanges)
        {
            var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges);
            if (tenant is null)
            {
                throw new TenantNotFoundException(tenantId);
            }
            
            return new TenantDto
            {
                Id = tenant.Id,
                Title = tenant.Title,
                Address = tenant.Address
            };
        }
    }
}
