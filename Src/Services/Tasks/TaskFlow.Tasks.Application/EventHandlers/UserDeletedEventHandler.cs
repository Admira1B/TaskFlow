using MediatR;
using TaskFlow.Identity.Contracts.Events;

namespace TaskFlow.Tasks.Application.EventHandlers {
    public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent> {
        public async Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken) {
            // TODO!!!
        }
    }
}
