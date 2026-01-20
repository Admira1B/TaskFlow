using Microsoft.Extensions.Options;
using TaskFlow.Shared.Messaging;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Identity.Application.Contracts;
using static TaskFlow.Shared.Messaging.Constants.RabbitMqConstants;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class IdentityServiceEventPublisher(IOptions<RabbitMqOptions> options) : RabbitMqEventPublisher(options), IEventPublisher {
        public async Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : BaseEvent {
            ArgumentNullException.ThrowIfNull(@event);

            return await base.PublishEventAsync(
                @event,
                exchange: IdentityService.ExchangeName,
                routingKey: routingKey,
                serviceName: IdentityService.ServiceName,
                ct
            );
        }
    }
}
