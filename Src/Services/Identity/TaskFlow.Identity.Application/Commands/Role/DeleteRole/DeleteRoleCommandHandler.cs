using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public class DeleteRoleCommandHandler(RoleManager<Domain.Entities.Role> manager) : IRequestHandler<DeleteRoleCommand, RequestResult<Unit>> {
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RequestResult<Unit>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken) {
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                return RequestResult<Unit>.NotFound("Role", command.Id);
            }

            var result = await _manager.DeleteAsync(role);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);

                return RequestResult<Unit>.Failure(string.Join(",", errors));
            }

            return RequestResult<Unit>.Success();
        }
    }
}
