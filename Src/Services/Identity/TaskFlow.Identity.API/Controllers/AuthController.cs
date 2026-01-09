using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Application.Commands.Auth.Logout;
using TaskFlow.Identity.Application.Commands.Auth.Register;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
            var command = _mapper.Map<RegisterCommand>(request);

            var result = await _mediator.Send(command);

            if (!result.Succeeded) {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            var command = _mapper.Map<LoginCommand>(request);

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
