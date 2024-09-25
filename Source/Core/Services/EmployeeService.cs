using Contracts;
using Entities.Entities;
using Entities.Enums;
using Entities.Exceptions;
using Entities.LinkModels;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IDataShaper<EmployeeDto> _dataShaper;
    private readonly IEmployeeLinks _employeeLinks;

    public EmployeeService(
        IRepositoryManager repository,
        ILoggerManager logger,
        IDataShaper<EmployeeDto> dataShaper,
        IEmployeeLinks employeeLinks)
    {
        _repository = repository;
        _logger = logger;
        _dataShaper = dataShaper;
        _employeeLinks = employeeLinks;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid tenantId,
        EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(tenantId, employeeParameters, trackChanges);
        var employeeDtos = employeesWithMetaData.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            MobileNo = e.MobileNo,
            BloodGroup = e.BloodGroup.ToString()
        }).ToList();

        return (employees: employeeDtos, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges)
    {
        var employee = await _repository.Employee.GetEmployeeAsync(tenantId, employeeId, trackChanges)
            ?? throw new EmployeeNotFoundException(employeeId);

        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            MobileNo = employee.MobileNo,
            BloodGroup = employee.BloodGroup.ToString()
        };
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(Guid tenantId, EmployeeForCreationDto employee, bool trackChanges)
    {
        var tenant = await _repository.Tenant.GetTenantAsync(tenantId, trackChanges)
            ?? throw new TenantNotFoundException(tenantId);

        BloodGroup bloodGrp;
        Enum.TryParse(employee.BloodGroup, out bloodGrp);

        var entity = new Employee
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            MobileNo = employee.MobileNo,
            TenantId = tenantId,
            BloodGroup = bloodGrp
        };
        _repository.Employee.CreateEmployee(entity);
        await _repository.SaveChangesAsync();

        return new EmployeeDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            MobileNo = entity.MobileNo
        };
    }

    public async Task DeleteEmployeeAsync(Guid tenantId, Guid employeeId, bool trackChanges)
    {
        var employee = await _repository.Employee.GetEmployeeAsync(tenantId, employeeId, trackChanges)
            ?? throw new EmployeeNotFoundException(employeeId);

        _repository.Employee.DeleteEmployee(employee);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Guid tenantId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges)
    {
        var employeeEntity = await _repository.Employee.GetEmployeeAsync(tenantId, employeeId, trackChanges)
            ?? throw new EmployeeNotFoundException(employeeId);

        BloodGroup bloodGrp;
        Enum.TryParse(employeeForUpdate.BloodGroup, out bloodGrp);

        employeeEntity.FirstName = employeeForUpdate.FirstName;
        employeeEntity.LastName = employeeForUpdate.LastName;
        employeeEntity.Email = employeeForUpdate.Email;
        employeeEntity.MobileNo = employeeForUpdate.MobileNo;
        employeeEntity.BloodGroup = bloodGrp;

        await _repository.SaveChangesAsync();
    }

    public async Task<(IEnumerable<ShapedEntity> employees, MetaData metaData)> GetEmployeesDataShaperAsync(Guid tenantId,
        EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(tenantId, employeeParameters, trackChanges);
        var employeeDtos = employeesWithMetaData.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            MobileNo = e.MobileNo,
            BloodGroup = e.BloodGroup.ToString()
        }).ToList();

        var shapedData = _dataShaper.ShapeData(employeeDtos, employeeParameters.Fields);

        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetHATEOASEmployeesAsync(Guid tenantId,
        EmployeeLinkParameters linkParameters, bool trackChanges)
    {
        if (!linkParameters.EmployeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(tenantId,
            linkParameters.EmployeeParameters, trackChanges);

        var employeeDtos = employeesWithMetaData.Select(e => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            MobileNo = e.MobileNo,
            BloodGroup = e.BloodGroup.ToString()
        }).ToList();

        var links = _employeeLinks.TryGenerateLinks(employeeDtos,
            linkParameters.EmployeeParameters.Fields, tenantId, linkParameters.Context);

        return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
    }
}
