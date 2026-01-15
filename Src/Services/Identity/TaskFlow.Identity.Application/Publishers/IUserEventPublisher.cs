using TaskFlow.Identity.Contracts.Events;

namespace TaskFlow.Identity.Application.Publishers {
    public interface IUserEventPublisher {
        Task<bool> PublishUserDeletedEvent(UserDeletedEvent @event, CancellationToken ct);
    }
}
