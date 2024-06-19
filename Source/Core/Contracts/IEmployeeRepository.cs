using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid tenantId, bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
    }
}
