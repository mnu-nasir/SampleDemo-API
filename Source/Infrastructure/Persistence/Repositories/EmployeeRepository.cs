using Contracts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;

namespace Persistence.Repositories
{
    internal sealed class EmployeeRepository : RepositoryBase<ApplicationContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid tenantId, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.TenantId.Equals(tenantId), trackChanges).ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges)
        {
            var employee = await FindByCondition(e => e.TenantId.Equals(tenantId)
                                    && e.Id.Equals(employeeId), trackChanges)
                .SingleOrDefaultAsync();
            return employee;
        }
    }
}
