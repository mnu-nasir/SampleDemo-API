using Contracts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DbContexts;
using Shared.RequestFeatures;

namespace Persistence.Repositories
{
    internal sealed class EmployeeRepository : RepositoryBase<ApplicationContext, Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid tenantId, EmployeeParameters employeeParameters, 
            bool trackChanges)
        {
            var employees = await FindByCondition(e => e.TenantId.Equals(tenantId), trackChanges)
                .OrderBy(e => e.FirstName)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(e => e.TenantId.Equals(tenantId), trackChanges).CountAsync();

            return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges)
        {
            var employee = await FindByCondition(e => e.TenantId.Equals(tenantId)
                                    && e.Id.Equals(employeeId), trackChanges)
                .SingleOrDefaultAsync();
            return employee;
        }

        public void CreateEmployee(Employee employee)
        {
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }
    }
}
