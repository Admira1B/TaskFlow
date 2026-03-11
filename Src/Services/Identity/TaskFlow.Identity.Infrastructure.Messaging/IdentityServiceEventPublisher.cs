using Microsoft.Extensions.Options;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Messaging.RabbitMQ.Services;
using static TaskFlow.Shared.Messaging.RabbitMQ.Constants.RabbitMqConstants;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class IdentityServiceEventPublisher(ILogger logger, IOptions<RabbitMqOptions> options) : RabbitMqEventPublisher(logger, options) {
        public override async Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) {
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
