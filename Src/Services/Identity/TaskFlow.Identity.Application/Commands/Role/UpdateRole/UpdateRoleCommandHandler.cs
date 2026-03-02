using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Identity.Application.Commands.Role.UpdateRole {
    public class UpdateRoleCommandHandler(ILogger logger, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<UpdateRoleCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RequestResult<Unit>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Role {RoleId} updating attempt", command.Id.ToString());
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                _logger.Debug("Role updating failed. Role {RoleId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Role", command.Id);
            }

            role.Description = command.Description;

            var result = await _manager.UpdateAsync(role);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(",", errors);

                _logger.Debug("Role updating failed. RoleId: {RoleId}, Exception: {Message}", command.Id.ToString(), message);
                return RequestResult<Unit>.Failure(message);
            }

            _logger.Debug("Role updated successfully. Name: {RoleName}, Description: {Description}", role.Name!, command.Description);

            return RequestResult<Unit>.Success();
        }
    }
}
