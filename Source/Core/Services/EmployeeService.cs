using Contracts;
using Entities.Entities;
using Entities.Enums;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Services
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid tenantId,
            EmployeeParameters employeeParameters, bool trackChanges)
        {
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

            var entity = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                MobileNo = employee.MobileNo,
                TenantId = tenantId
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
    }
}
