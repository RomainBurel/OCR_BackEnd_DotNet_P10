using IdentityAPI.Models;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
        {
            this._authorizationService = authorizationService;
            this._logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var userToken = await _authorizationService.GetLoginToken(model);
            if (userToken != null)
            {
                this._logger.LogInformation($"Successfull login for user with name: {model.Username}");

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(userToken),
                    expiration = userToken.ValidTo
                });
            }

            _logger.LogWarning($"Login failed for user with mail: {model.Username}");
            return Unauthorized();
        }
    }
}
