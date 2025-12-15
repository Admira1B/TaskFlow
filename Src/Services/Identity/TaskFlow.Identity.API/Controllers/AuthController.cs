using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Application.Commands.Auth;
using TaskFlow.Identity.Application.Services;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AuthService service) : ControllerBase {
        private readonly AuthService _service = service;

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command) {
            var result = await _service.RegisterAsync(command);

            if (!result.Succeeded) {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command) { 
            var result = await _service.LoginAsync(command);

            if (!result.Succeeded) {
                return Unauthorized(result.ErrorMessage);
            }

            return Ok(result.User);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() { 
            await _service.LogoutAsync();

            return Ok();
        }
    }
}
