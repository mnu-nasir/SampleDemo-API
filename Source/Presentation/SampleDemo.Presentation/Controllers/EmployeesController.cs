using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace SampleDemo.Presentation.Controllers
{
    [Route("api/tenants/{tenantId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForTenant([FromRoute] Guid tenantId)
        {
            var employees = await _service.EmployeeService.GetEmployees(tenantId, false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeForTenant([FromRoute] Guid tenantId, [FromRoute] Guid id)
        {
            var employee = await _service.EmployeeService.GetEmployee(tenantId, id, false);
            return Ok(employee);
        }
    }
}
