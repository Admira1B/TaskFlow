using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskFlow.Identity.Domain.Options;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Application.Commands.Auth.Logout;
using TaskFlow.Identity.Application.Commands.Auth.Register;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IMediator mediator, IMapper mapper, IOptions<JsonWebTokenOptions> jwtOptions) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly JsonWebTokenOptions _jwtOptions = jwtOptions.Value;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Application.DTOs.Requests.Auth.RegisterRequest request) {
            var command = _mapper.Map<RegisterCommand>(request);

            var result = await _mediator.Send(command);

            if (!result.Succeeded) {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(
                new {
                    user = result.User,
                    token = result.Token,
                    expiresIn = _jwtOptions.ExpiresHours * 3600
                }
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.DTOs.Requests.Auth.LoginRequest request) {
            var command = _mapper.Map<LoginCommand>(request);

            var result = await _mediator.Send(command);

            if (!result.Succeeded) {
                return Unauthorized(result.ErrorMessage);
            }

            return Ok(
                new {
                    user = result.User,
                    token = result.Token,
                    expiresIn = _jwtOptions.ExpiresHours * 3600
                }
            );
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _mediator.Send(new LogoutCommand());

            return Ok();
        }
    }
}
