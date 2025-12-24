using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Tasks.API.Extensions;
using TaskFlow.Tasks.Application.Queries.TaskItem.GetById;
using TaskFlow.Tasks.Application.Queries.TaskItem.GetByAssignee;
using TaskFlow.Tasks.Application.Queries.TaskItem.GetByReporter;
using TaskFlow.Tasks.Application.Queries.TaskItem.GetByProject;
using TaskFlow.Tasks.Application.Queries.TaskItem.GetByTaskGroup;
using TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem;
using TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem;
using TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem;

namespace TaskFlow.Tasks.API.Controllers {
    [ApiController]
    [Route("api/tasks")]
    public class TaskItemController(IMediator mediator) : ControllerBase {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) { 
            var query = new GetTaskItemByIdQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-assignee/{assigneeId:guid}")]
        public async Task<IActionResult> GetByAssignee([FromRoute] Guid assigneeId) { 
            var query = new GetTaskItemsByAssigneeQuery(assigneeId);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-reporter/{reporterId:guid}")]
        public async Task<IActionResult> GetByReporter([FromRoute] Guid reporterId) {
            var query = new GetTaskItemsByReporterQuery(reporterId);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-project/{projectId:guid}")]
        public async Task<IActionResult> GetByProject([FromRoute] Guid projectId) {
            var query = new GetTaskItemsByProjectQuery(projectId);
            
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-group/{groupId:guid}")]
        public async Task<IActionResult> GetByGroup([FromRoute] Guid groupId) {
            var query = new GetTaskItemsByTaskGroupQuery(groupId);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemCommand command) {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTaskItemCommand command) { 
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteTaskItemCommand(id);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
