using Asp.Versioning;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleDemo.Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace SampleDemo.Presentation.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/{v:apiversion}/tenants/{tenantId}/employees")]
[ApiController]
public class EmployeesV2Controller : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesV2Controller(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesForTenant([FromRoute] Guid tenantId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _service.EmployeeService.GetEmployeesAsync(tenantId, employeeParameters, false);

        //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeById")]
    public async Task<IActionResult> GetEmployeeForTenant([FromRoute] Guid tenantId, [FromRoute] Guid id)
    {
        var employee = await _service.EmployeeService.GetEmployeeAsync(tenantId, id, false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForTenant([FromRoute] Guid tenantId, [FromBody] EmployeeForCreationDto employee)
    {
        //if (employee is null)
        //    return BadRequest("EmployeeForCreationDto object is null");

        //if (!ModelState.IsValid)
        //    return UnprocessableEntity(ModelState);

        var employeeToReturn = await _service.EmployeeService.CreateEmployeeAsync(tenantId, employee, false);

        return CreatedAtRoute("GetEmployeeById", new { tenantId, id = employeeToReturn.Id }, employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForTenant([FromRoute] Guid tenantId, [FromRoute] Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeAsync(tenantId, id, true);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployee([FromRoute] Guid tenantId, [FromRoute] Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        //if (employee is null)
        //    return BadRequest("EmployeeForUpdateDto object is null");

        //if (!ModelState.IsValid)
        //    return UnprocessableEntity(ModelState);

        await _service.EmployeeService.UpdateEmployeeAsync(tenantId, id, employee, true);

        return NoContent();
    }

    [HttpGet("shapedAll")]
    public async Task<IActionResult> GetShapedEmployeesForTenant([FromRoute] Guid tenantId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _service.EmployeeService.GetEmployeesDataShaperAsync(tenantId, employeeParameters, false);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.employees);
    }

    [HttpGet("hateoasAll")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetHATEOASEmployeesForTenant([FromRoute] Guid tenantId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var linkParams = new EmployeeLinkParameters(employeeParameters, HttpContext);

        var result = await _service.EmployeeService.GetHATEOASEmployeesAsync(tenantId, linkParams, trackChanges: false);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.metaData));

        return result.linkResponse.HasLinks
                    ? Ok(result.linkResponse.LinkedEntities)
                    : Ok(result.linkResponse.ShapedEntities);
    }
}
