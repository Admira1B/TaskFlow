using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Application.Queries.User;
using TaskFlow.Identity.Application.Commands.User;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UserController(UserService service) : ControllerBase {
        private readonly UserService _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetUserByIdQuery(id);

            var result = await _service.GetByIdAsync(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email) {
            var query = new GetUserByEmailQuery(email);

            var result = await _service.GetByEmailAsync(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name) {
            var query = new GetUserByUserNameQuery(name);

            var result = await _service.GetByUserNameAsync(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] GetUsersPaginatedQuery query) {
            var result = await _service.GetPaginatedAsync(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command) { 
            var result = await _service.CreateAsync(command);

            if (result is null) {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command) { 
            var result = await _service.UpdateAsync(command);

            if (!result) {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var command = new DeleteUserCommand(id);

            var result = await _service.DeleteAsync(command);

            if (!result) {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
