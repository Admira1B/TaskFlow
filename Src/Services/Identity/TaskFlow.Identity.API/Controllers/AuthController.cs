using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Identity.API.Extensions;
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
            return result.ToActionResult(_jwtOptions);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.DTOs.Requests.Auth.LoginRequest request) {
            var command = _mapper.Map<LoginCommand>(request);

            var result = await _mediator.Send(command);
            return result.ToActionResult(_jwtOptions);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout() {
            await _mediator.Send(new LogoutCommand());
            return Ok();
        }
    }
}
