using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployees(Guid tenantId, bool trackChanges);
        Task<EmployeeDto> GetEmployee(Guid tenantId, Guid employeeId, bool trackChanges);
    }
}
