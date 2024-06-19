using Contracts;
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
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees(Guid tenantId, bool trackChanges)
        {
            var employees = await _repository.Employee.GetEmployeesAsync(tenantId, trackChanges);
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                MobileNo = e.MobileNo,
                BloodGroup = e.BloodGroup.ToString()
            }).ToList();
        }

        public async Task<EmployeeDto> GetEmployee(Guid tenantId, Guid employeeId, bool trackChanges)
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
    }
}
