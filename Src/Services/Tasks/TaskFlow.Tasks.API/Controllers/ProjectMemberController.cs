using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Tasks.Contracts.DTOs.Requests.ProjectMember;
using TaskFlow.Tasks.Application.Queries.ProjectMember.GetById;
using TaskFlow.Tasks.Application.Queries.ProjectMember.GetByUser;
using TaskFlow.Tasks.Application.Queries.ProjectMember.GetByProject;
using TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember;
using TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember;
using TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember;

namespace TaskFlow.Tasks.API.Controllers {
    [ApiController]
    [Route("api/members")]
    [Authorize]
    public class ProjectMemberController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct) {
            var query = new GetProjectMemberByIdQuery(id);

            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpGet("by-user/{userId:guid}")]
        public async Task<IActionResult> GetByUser([FromRoute] Guid userId, CancellationToken ct) {
            var query = new GetProjectsMembersByUserQuery(userId);

            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpGet("by-project/{projectId:guid}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId, CancellationToken ct) {
            var query = new GetProjectsMembersByProjectQuery(projectId);

            var result = await _mediator.Send(query, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectMemberRequest request, CancellationToken ct) {
            var command = _mapper.Map<CreateProjectMemberCommand>(request);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProjectMemberRequest request, CancellationToken ct) {
            var command = _mapper.Map<UpdateProjectMemberCommand>(
                request,
                opts => opts.Items[nameof(UpdateProjectMemberCommand.Id)] = id
            );

            var result = await _mediator.Send(command, cancellationToken: ct);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct) {
            var command = new DeleteProjectMemberCommand(id);

            var result = await _mediator.Send(command, cancellationToken: ct);
            return result.ToActionResult();
        }
    }
}
