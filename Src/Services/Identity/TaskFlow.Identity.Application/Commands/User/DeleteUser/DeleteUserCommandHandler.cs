using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Contracts.Events;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.Publishers;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public class DeleteUserCommandHandler(UserManager<Domain.Entities.User> manager, IEventPublisher publisher) : IRequestHandler<DeleteUserCommand, RequestResult<Unit>> {
        private readonly UserManager<Domain.Entities.User> _manager = manager;
        private readonly IEventPublisher _publisher = publisher;

        public async Task<RequestResult<Unit>> Handle(DeleteUserCommand command, CancellationToken cancellationToken) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return RequestResult<Unit>.NotFound("User", command.Id);
            }

            var isPublished = await _publisher.PublishUserDeletedEvent(
                new UserDeletedEvent {
                    UserId = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                }
            );

            if (!isPublished) {
                return RequestResult<Unit>.FailedToPublishEvent($"Failed to publish delete event for user {user.Id}");
            }

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);

                return RequestResult<Unit>.Failure(string.Join(",", errors));
            }

            return RequestResult<Unit>.Success();
        }
    }
}
