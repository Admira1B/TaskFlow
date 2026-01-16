using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using TaskFlow.Identity.Domain.Constants;
using TaskFlow.Identity.Contracts.Events;
using TaskFlow.Identity.Application.Publishers;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Messaging.Constants;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class EventPublisher : IEventPublisher, IDisposable {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMqOptions _options;
        private bool _disposed = false;

        public EventPublisher(IOptions<RabbitMqOptions> options) {
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

        public async ValueTask DisposeAsync() {
            if (!_disposed) {
                if (_channel != null && _channel.IsOpen) {
                    await _channel.CloseAsync();
                    _channel.Dispose();
                }

                if (_connection != null && _connection.IsOpen) {
                    await _connection.CloseAsync();
                    _connection.Dispose();
                }

                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        public void Dispose() {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        ~EventPublisher() {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        // Events
        public async Task<bool> PublishUserDeletedEvent(UserDeletedEvent @event, CancellationToken ct = default) {
            if (@event is null) {
                throw new ArgumentNullException(nameof(UserDeletedEvent));
            }

            try {
                @event.SourceService = ServiceConstants.ServiceName;

                var jsonEvent = JsonSerializer.Serialize(@event);

                var props = new BasicProperties() {
                    Persistent = true,
                    MessageId = @event.EventId.ToString(),
                    Timestamp = new AmqpTimestamp(new DateTimeOffset(@event.OccurredOn).ToUnixTimeSeconds()),
                    Type = @event.EventType,
                    ContentType = "application/json"
                };

                await _channel.BasicPublishAsync(
                    exchange: RabbitMQConstants.IdentityService.ExchangeName,
                    routingKey: RabbitMQConstants.IdentityService.RoutingKeys.UserDeleted,
                    mandatory: true,
                    basicProperties: props,
                    body: new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(jsonEvent)),
                    cancellationToken: ct
                );

                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}
