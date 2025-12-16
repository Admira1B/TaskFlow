using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Application.Commands.Auth.Logout;
using TaskFlow.Identity.Application.Commands.Auth.Register;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator) : ControllerBase {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command) {
            var result = await _mediator.Send(command);

            if (!result.Succeeded) {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command) { 
            var result = await _mediator.Send(command);

            if (!result.Succeeded) {
                return Unauthorized(result.ErrorMessage);
            }

            return Ok(result.User);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _mediator.Send(new LogoutCommand());

            return Ok();
        }
    }
}
