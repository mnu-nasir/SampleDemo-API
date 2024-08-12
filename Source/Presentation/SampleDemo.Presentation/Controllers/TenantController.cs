using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SampleDemo.Presentation.ActionFilters;
using SampleDemo.Presentation.ModelBinders;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace SampleDemo.Presentation.Controllers
{
    [Route("api/tenants")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TenantController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants(TenantParameters tenantParameters)
        {
            try
            {
                var pagedResult = await _serviceManager.TenantService.GetAllTenantsAsync(tenantParameters, false);

                //Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
                Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

                return Ok(pagedResult.tenants);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:guid}", Name = "GetTenantById")]
        public async Task<IActionResult> GetTenant([FromRoute] Guid id)
        {
            var tenant = await _serviceManager.TenantService.GetTenantAsync(id, false);
            return Ok(tenant);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTenant([FromBody] TenantForCreationDto tenant)
        {
            //if (tenant == null)
            //    return BadRequest("TenantForCreationDto object is null");

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            var createdTenant = await _serviceManager.TenantService.CreateTenantAsync(tenant);

            return CreatedAtRoute("GetTenantById", new { id = createdTenant.Id }, createdTenant);
        }

        [HttpGet("collection/{ids}", Name = "GetTenantCollection")]
        public async Task<IActionResult> GetTenantCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var tenants = await _serviceManager.TenantService.GetTenantsByIdAsync(ids, false);
            return Ok(tenants);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTenantCollection(IEnumerable<TenantForCreationDto> tenantCollection)
        {
            //if (tenantCollection is null)
            //    return BadRequest("tenantCollection is empty");

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            var result = await _serviceManager.TenantService.CreateTenantCollection(tenantCollection);
            return CreatedAtRoute("GetTenantCollection", new { ids = result.ids }, result.tenants);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            await _serviceManager.TenantService.DeleteTenantAsync(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTenant([FromRoute] Guid id, [FromBody] TenantForUpdateDto tenant)
        {
            //if (tenant is null)
            //    return BadRequest("TenantForUpdateDto is empty");

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity();

            await _serviceManager.TenantService.UpdateTenantAsync(id, tenant, true);

            return NoContent();
        }

        [HttpGet("shappedAll")]
        public async Task<IActionResult> GetDataShapedTenants([FromQuery] TenantParameters tenantParameters)
        {
            try
            {
                var pagedResult = await _serviceManager.TenantService.GetAllDataShapedTenantsAsync(tenantParameters, false);

                Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

                return Ok(pagedResult.tenants);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("hateoasAll")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetHATEOASTenants([FromQuery] TenantParameters tenantParameters)
        {
            var linkParams = new TenantLinkParameters(tenantParameters, HttpContext);
            
            var result = await _serviceManager.TenantService.GetHATEOASAllTenantsAsync(linkParams, false);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks
                            ? Ok(result.linkResponse.LinkedEntities)
                            : Ok(result.linkResponse.ShapedEntities);
        }
    }
}