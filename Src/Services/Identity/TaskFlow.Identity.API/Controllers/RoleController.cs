using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Identity.API.Extensions;
using TaskFlow.Identity.Contracts.DTOs.Requests.Role;
using TaskFlow.Identity.Application.Queries.Role.GetById;
using TaskFlow.Identity.Application.Queries.Role.GetByName;
using TaskFlow.Identity.Application.Queries.Role.GetPaginated;
using TaskFlow.Identity.Application.Commands.Role.CreateRole;
using TaskFlow.Identity.Application.Commands.Role.UpdateRole;
using TaskFlow.Identity.Application.Commands.Role.DeleteRole;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetRoleByIdQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name) {
            var query = new GetRoleByNameQuery(name);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] GetRolesPaginatedRequest request) {
            var query = _mapper.Map<GetRolesPaginatedQuery>(request);
            
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request) {
            var command = _mapper.Map<CreateRoleCommand>(request);

            var result = await _mediator.Send(command);

            if (result.Succeeded) {
                return CreatedAtAction(nameof(GetById), new { result.Value!.Id }, result.Value);  
            }

            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleRequest request) {
            var command = _mapper.Map<UpdateRoleCommand>(
                request,
                opts => opts.Items[nameof(UpdateRoleCommand.Id)] = id
            );

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 
            var command = new DeleteRoleCommand(id);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
