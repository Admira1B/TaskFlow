using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Application.Commands.Role;
using TaskFlow.Identity.Application.Queries.Role;
using TaskFlow.Identity.Application.Services;

namespace TaskFlow.Identity.API.Controllers {
    [ApiController]
    [Route("api/roles")]
    public class RoleController(RoleService service) : ControllerBase {
        private readonly RoleService _service = service;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var query = new GetRoleByIdQuery(id);

            var result = await _service.GetByIdAsync(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name) {
            var query = new GetRoleByNameQuery(name);

            var result = await _service.GetByNameAsync(query);

            if (result is null) {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated([FromQuery] GetRolesPaginatedQuery query) { 
            var result = await _service.GetPaginatedAsync(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateRoleCommand command) {
            var result = await _service.CreateAsync(command);

            if (result is null) {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { result.Id }, result);  
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command) {
            var result = await _service.UpdateAsync(command);

            if (result == false) {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 
            var command = new DeleteRoleCommand(id);

            var result = await _service.DeleteAsync(command);

            if (result == false) {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
