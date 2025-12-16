using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Application.Commands.User.UpdateUser;
using TaskFlow.Identity.Application.Commands.User.DeleteUser;
using TaskFlow.Identity.Application.Queries.User.GetById;
using TaskFlow.Identity.Application.Queries.User.GetByName;
using TaskFlow.Identity.Application.Queries.User.GetByEmail;
using TaskFlow.Identity.Application.Queries.User.GetPaginated;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UserController(IMediator mediator) : ControllerBase {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetUserByIdQuery(id);

            var result = await _mediator.Send(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email) {
            var query = new GetUserByEmailQuery(email);

            var result = await _mediator.Send(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name) {
            var query = new GetUserByUserNameQuery(name);

            var result = await _mediator.Send(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] GetUsersPaginatedQuery query) {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command) { 
            var result = await _mediator.Send(command);

            if (!result) {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteUserCommand(id);

            var result = await _mediator.Send(command);

            if (!result) {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
