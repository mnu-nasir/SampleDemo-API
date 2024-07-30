using Entities.Entities;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid tenantId,
            EmployeeParameters employeeParameters, bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeAsync(Guid tenantId, EmployeeForCreationDto employee, bool trackChanges);
        Task DeleteEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
        Task UpdateEmployeeAsync(Guid tenantId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges);

        Task<(IEnumerable<Entity> employees, MetaData metaData)> GetEmployeesDataShaperAsync(Guid tenantId,
            EmployeeParameters employeeParameters, bool trackChanges);
    }
}
