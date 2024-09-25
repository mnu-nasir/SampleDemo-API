using Entities.Entities;
using Shared.RequestFeatures;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(Guid tenantId, EmployeeParameters employeeParameters,
        bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Employee employee);
    void DeleteEmployee(Employee employee);
}
