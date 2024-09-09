using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleDemo.Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace SampleDemo.Presentation.Controllers.V1
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationV1Controller : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationV1Controller(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _serviceManager.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            return Ok(new
            {
                Token = await _serviceManager.AuthenticationService.CreateToken()
            });
        }

        [HttpPost("loginWithRefreshToken")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AuthenticateWithRefreshToken([FromBody] UserForAuthenticationDto user)
        {
            if (!await _serviceManager.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            var tokenDto = await _serviceManager.AuthenticationService.CreateAllToken(populateExp: true);

            return Ok(tokenDto);
        }
    }
}
