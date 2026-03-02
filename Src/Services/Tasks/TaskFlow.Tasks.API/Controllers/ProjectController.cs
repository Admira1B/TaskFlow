using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Tasks.Contracts.DTOs.Requests.Project;
using TaskFlow.Tasks.Application.Queries.Project.GetById;
using TaskFlow.Tasks.Application.Queries.Project.GetByOwner;
using TaskFlow.Tasks.Application.Queries.Project.GetByProjectMember;
using TaskFlow.Tasks.Application.Commands.Project.CreateProject;
using TaskFlow.Tasks.Application.Commands.Project.UpdateProject;
using TaskFlow.Tasks.Application.Commands.Project.DeleteProject;

namespace TaskFlow.Tasks.API.Controllers {
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct) {
            var query = new GetProjectByIdQuery(id);
            
            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpGet("by-owner/{ownerId:guid}")]
        public async Task<IActionResult> GetByOwner([FromRoute] Guid ownerId, CancellationToken ct) {
            var query = new GetProjectsByOwnerQuery(ownerId);
            
            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpGet("by-member/{memberId:guid}")]
        public async Task<IActionResult> GetByProjectMember([FromRoute] Guid memberId, CancellationToken ct) {
            var query = new GetProjectsByProjectMemberQuery(memberId);
            
            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct) {
            var command = _mapper.Map<CreateProjectCommand>(
                request,
                opts => opts.Items[nameof(CreateProjectCommand.OwnerId)] = User.GetUserId()
            );

            var result = await _mediator.Send(command, cancellationToken: ct);

            if (result.Succeeded) {
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Value!.Id },
                    result.Value
                );
            }

            return result.ToActionResult();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProjectRequest request, CancellationToken ct) {
            var command = _mapper.Map<UpdateProjectCommand>(
                request,
                opts => opts.Items[nameof(UpdateProjectCommand.Id)] = id
            );

            var result = await _mediator.Send(command, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct) {
            var command = new DeleteProjectCommand(id);

            var result = await _mediator.Send(command, cancellationToken: ct);
            return result.ToActionResult();
        }
    }
}
