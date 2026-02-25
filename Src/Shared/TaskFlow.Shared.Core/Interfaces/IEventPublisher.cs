using TaskFlow.Shared.Core.Abstractions;

namespace TaskFlow.Shared.Core.Interfaces {
    public interface IEventPublisher {
        Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : EventBase;
    }
}
