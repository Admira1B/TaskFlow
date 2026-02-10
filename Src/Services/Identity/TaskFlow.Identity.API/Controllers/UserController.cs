using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Identity.API.Extensions;
using TaskFlow.Identity.Contracts.DTOs.Requests.User;
using TaskFlow.Identity.Application.Commands.User.UpdateUser;
using TaskFlow.Identity.Application.Commands.User.DeleteUser;
using TaskFlow.Identity.Application.Queries.User.Exists;
using TaskFlow.Identity.Application.Queries.User.GetById;
using TaskFlow.Identity.Application.Queries.User.GetByEmail;
using TaskFlow.Identity.Application.Queries.User.GetByName;
using TaskFlow.Identity.Application.Queries.User.GetPaginated;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("exists/{id:guid}")]
        public async Task<IActionResult> Exists([FromRoute] Guid id) {
            var query = new UserExistsQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetUserByIdQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email) {
            var query = new GetUserByEmailQuery(email);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name) {
            var query = new GetUserByUserNameQuery(name);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] GetUsersPaginatedRequest request) {
            var query = _mapper.Map<GetUsersPaginatedQuery>(request);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request) { 
            var command = _mapper.Map<UpdateUserCommand>(
                request,
                opts => opts.Items[nameof(UpdateUserCommand.Id)] = id
            );

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteUserCommand(id);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
