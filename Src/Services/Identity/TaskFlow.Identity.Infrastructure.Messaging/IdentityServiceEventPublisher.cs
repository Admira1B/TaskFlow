using Microsoft.Extensions.Options;
using TaskFlow.Shared.Core.Entities;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging;
using TaskFlow.Shared.Messaging.Options;
using static TaskFlow.Shared.Messaging.Constants.RabbitMqConstants;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class IdentityServiceEventPublisher(ILogger logger, IOptions<RabbitMqOptions> options) : RabbitMqEventPublisher(logger, options), IEventPublisher {
        public async Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : EventBase {
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
