using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Contracts.Events;
using static TaskFlow.Shared.Messaging.RabbitMQ.Constants.RabbitMqConstants.IdentityService;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public class DeleteUserCommandHandler(ILogger logger, UserManager<Domain.Entities.User> manager, IEventPublisher publisher) : IRequestHandler<DeleteUserCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly UserManager<Domain.Entities.User> _manager = manager;
        private readonly IEventPublisher _publisher = publisher;

        public async Task<RequestResult<Unit>> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("User {UserId} deletion attempt", command.Id);
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                _logger.Debug("User deletion failed. User {UserId} not found", command.Id);
                return RequestResult<Unit>.NotFound("User", command.Id);
            }

            var isPublished = await _publisher.PublishEventAsync(
                new UserDeletedEvent {
                    UserId = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                },
                routingKey: RoutingKeys.UserDeleted,
                cancellationToken
            );

            if (!isPublished) {
                _logger.Error("User deletion failed. Failed to publish delete event for user {UserId}", null, command.Id);
                return RequestResult<Unit>.FailedToPublishEvent($"Failed to publish delete event for user {user.Id}");
            }

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(",", errors);

                _logger.Debug("User deletion failed. UserId: {UserId}, Error: {Message}", command.Id, message);

                return RequestResult<Unit>.Failure(message);
            }

            _logger.Debug("User {UserId} was successfully deleted", user.Id);

            return RequestResult<Unit>.Success();
        }
    }
}
