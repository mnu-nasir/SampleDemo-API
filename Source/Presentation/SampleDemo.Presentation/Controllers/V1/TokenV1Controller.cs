using Microsoft.AspNetCore.Mvc;
using SampleDemo.Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace SampleDemo.Presentation.Controllers.V1;

[Route("api/authentication")]
[ApiController]
public class TokenV1Controller : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public TokenV1Controller(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}
