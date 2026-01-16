using TaskFlow.Identity.Contracts.Events;

namespace TaskFlow.Identity.Application.Publishers {
    public interface IEventPublisher {
        Task<bool> PublishUserDeletedEvent(UserDeletedEvent @event, CancellationToken ct = default);
    }
}
