using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public class DeleteRoleCommandHandler(RoleManager<Domain.Entities.Role> manager) : IRequestHandler<DeleteRoleCommand, bool> {
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken cancellationToken) {
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                return false;
            }

            var result = await _manager.DeleteAsync(role);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }
    }
}
