using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TaskFlow.Identity.Application.Publishers;
using TaskFlow.Identity.Contracts.Events;
using TaskFlow.Identity.Domain.Constants;
using TaskFlow.Shared.Messaging.Constants;
using TaskFlow.Shared.Messaging.Options;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class RabbitMQUserEventPublisher : IUserEventPublisher, IDisposable {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMQOptions _options;
        private readonly bool _disposed = false;

        public RabbitMQUserEventPublisher(IOptions<RabbitMQOptions> options) {
            _options = options.Value;

            var factory = new ConnectionFactory {
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                HostName = _options.HostName,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(
                exchange: RabbitMQConstants.IdentityService.ExchangeName,
                type: RabbitMQConstants.TopicExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            ).GetAwaiter().GetResult();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        // Events
        public async Task<bool> PublishUserDeletedEvent(UserDeletedEvent @event, CancellationToken ct = default) {
            if (@event is null) {
                throw new ArgumentNullException(nameof(UserDeletedEvent));
            }

            @event.SourceService = ServiceConstants.ServiceName;

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            // NOT FINISHED
        }
    }
}
