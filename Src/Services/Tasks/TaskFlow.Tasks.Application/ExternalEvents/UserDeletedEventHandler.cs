using MediatR;
using TaskFlow.Identity.Contracts.Events;

namespace TaskFlow.Tasks.Application.ExternalEvents {
    public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent> {
        public async Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken = default) {
            // TODO: Add processing of comments, projects, tasks of deleted Users!!!
        }
    }
}
