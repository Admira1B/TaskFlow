using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.User.UpdateUser {
    public class UpdateUserCommandHandler(ILogger logger, UserManager<Domain.Entities.User> manager) : IRequestHandler<UpdateUserCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly UserManager<Domain.Entities.User> _manager = manager;
        
        public async Task<RequestResult<Unit>> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("User {UserId} updating attempt", command.Id.ToString());
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                _logger.Debug("User updating failed. User {UserId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("User", command.Id);
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _manager.UpdateAsync(user);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(",", errors);

                _logger.Debug("User updating failed. UserId: {UserId}, Exception: {Message}", command.Id.ToString(), message);
                return RequestResult<Unit>.Failure(message);
            }
            
            _logger.Debug("User updated successfully. Name: {UserName}, Email: {Email}", user.UserName!, user.Email!);

            return RequestResult<Unit>.Success();
        }
    }
}
