using Microsoft.Extensions.Options;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Messaging.RabbitMQ.Constants;
using TaskFlow.Shared.Messaging.RabbitMQ.Services;

namespace TaskFlow.Tasks.Infrastructure.Messaging {
    public class TasksServiceEventConsumer(ILogger logger, IOptions<RabbitMqOptions> options, IServiceProvider services) 
        : RabbitMqEventConsumer(logger, options, services, new Dictionary<string, string> {
            // Adding subscriptions to events
            [RabbitMqConstants.IdentityService.ExchangeName] = RabbitMqConstants.IdentityService.RoutingPattern,
        }) {
    }
}
