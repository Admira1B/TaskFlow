using TaskFlow.Shared.Messaging;

namespace TaskFlow.Identity.Application.Contracts {
    public interface IEventPublisher {
        Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : BaseEvent;
    }
}
