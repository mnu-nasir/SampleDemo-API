using Entities.Entities;
using Entities.LinkModels;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid tenantId,
        EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid tenantId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges);
    Task UpdateEmployeeAsync(Guid tenantId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges);

    Task<(IEnumerable<ShapedEntity> employees, MetaData metaData)> GetEmployeesDataShaperAsync(Guid tenantId,
        EmployeeParameters employeeParameters, bool trackChanges);

    /// <summary>
    /// For HATEOAS Implementation
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="linkParameters"></param>
    /// <param name="trackChanges"></param>
    /// <returns></returns>
    Task<(LinkResponse linkResponse, MetaData metaData)> GetHATEOASEmployeesAsync(Guid tenantId,
        EmployeeLinkParameters linkParameters, bool trackChanges);
}
