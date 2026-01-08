using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Tasks.API.Extensions;
using TaskFlow.Tasks.Application.DTOs.Requests.Comment;
using TaskFlow.Tasks.Application.Queries.Comment.GetById;
using TaskFlow.Tasks.Application.Queries.Comment.GetByTask;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Application.Commands.Comment.UpdateComment;
using TaskFlow.Tasks.Application.Commands.Comment.DeleteComment;

namespace TaskFlow.Tasks.API.Controllers {
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentController(IMediator mediator, IMapper mapper) : ControllerBase {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetCommentByIdQuery(id);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("by-task/{taskId:guid}")]
        public async Task<IActionResult> GetByTask([FromRoute] Guid taskId) {
            var query = new GetCommentsByTaskItemQuery(taskId);

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request) {
            var command = _mapper.Map<CreateCommentCommand>(
                request,
                opts => opts.Items["CurrentUserId"] = User.GetUserId()
            );

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentRequest request) {
            var command = _mapper.Map<UpdateCommentCommand>(
                request,
                opts => opts.Items[nameof(UpdateCommentCommand.Id)] = id
            );

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteCommentCommand(id);

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
