using Microsoft.Extensions.Options;
using TaskFlow.Shared.Logging;
using TaskFlow.Shared.Messaging;
using TaskFlow.Shared.Messaging.Constants;
using TaskFlow.Shared.Messaging.Options;

namespace TaskFlow.Tasks.Infrastructure.Messaging {
    public class TasksServiceEventConsumer(Logger logger, IOptions<RabbitMqOptions> options, IServiceProvider services) 
        : RabbitMqEventConsumer(logger, options, services, new Dictionary<string, string> {
            // Adding subscriptions to events
            [RabbitMqConstants.IdentityService.ExchangeName] = RabbitMqConstants.IdentityService.RoutingPattern,
        }) {
    }
}
