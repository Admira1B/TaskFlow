using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Tasks.API.Extensions;
using TaskFlow.Tasks.Application.DTOs.Requests.TaskGroup;
using TaskFlow.Tasks.Application.Queries.TaskGroup.GetById;
using TaskFlow.Tasks.Application.Queries.TaskGroup.GetByProject;
using TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup;
using TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup;
using TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup;

namespace TaskFlow.Tasks.API.Controllers {
    [ApiController]
    [Route("api/groups")]
    [Authorize]
    public class TaskGroupController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetTaskGroupByIdQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-project/{projectId:guid}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId) {
            var query = new GetTaskGroupsByProjectQuery(projectId);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupRequest request) {
            var command = _mapper.Map<CreateTaskGroupCommand>(request);

            var result = await _mediator.Send(command);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskGroupRequest request) {
            var command = _mapper.Map<UpdateTaskGroupCommand>(
                request,
                opts => opts.Items[nameof(UpdateTaskGroupCommand.Id)] = id
            );
            
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteTaskGroupCommand(id);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
