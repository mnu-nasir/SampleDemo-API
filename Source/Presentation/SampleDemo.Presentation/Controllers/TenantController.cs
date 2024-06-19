using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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
        public async Task<IActionResult> GetTenants()
        {
            try
            {
                var tenants = await _serviceManager.TenantService.GetAllTenantsAsync(false);
                return Ok(tenants);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTenant([FromRoute] Guid id)
        {
            var tenant = await _serviceManager.TenantService.GetTenantAsync(id, false);
            return Ok(tenant);
        }
    }
}
