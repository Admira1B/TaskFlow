using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public class DeleteRoleCommandHandler(ILogger logger, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<DeleteRoleCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RequestResult<Unit>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Role {RoleId} deletion attempt", command.Id.ToString());

            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
            _logger.Debug("Role deletion failed. Role {RoleId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Role", command.Id);
            }

            var result = await _manager.DeleteAsync(role);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(",", errors);

                _logger.Debug("Role deletion failed. RoleId: {RoleId}, Exception: {Message}", command.Id.ToString(), message);

                return RequestResult<Unit>.Failure(message);
            }

            _logger.Debug("Role {RoleId} deleted successfully. Name: {RoleName}, Description: {Description}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
