using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Application.Commands.Role.UpdateRole {
    public class UpdateRoleCommandHandler(RoleManager<Domain.Entities.Role> manager) : IRequestHandler<UpdateRoleCommand, bool> {
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<bool> Handle(UpdateRoleCommand command, CancellationToken cancellationToken) {
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                return false;
            }

            role.Description = command.Description;

            var result = await _manager.UpdateAsync(role);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }
    }
}
