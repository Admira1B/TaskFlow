using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using TaskFlow.Shared.Messaging.Events;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Identity.Application.Contracts;
using static TaskFlow.Shared.Messaging.Constants.RabbitMqConstants;

namespace TaskFlow.Identity.Infrastructure.Messaging {
    public class EventPublisher : IDisposable, IEventPublisher {
        private readonly IChannel _channel;
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly HashSet<string> _declaredExchanges;

        private bool _disposed = false;

        public EventPublisher(IOptions<RabbitMqOptions> options) {
            _declaredExchanges = [];
            _options = options.Value;
            _jsonOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var factory = new ConnectionFactory {
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                HostName = _options.HostName,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task<bool> PublishEventAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : BaseEvent {
            ArgumentNullException.ThrowIfNull(@event);

            try {
                var exchange = IdentityService.ExchangeName;
                await EnsureExchangeExistsAsync(exchange, ct);

                @event.SourceService = IdentityService.ServiceName;
                var jsonEvent = JsonSerializer.Serialize(@event, _jsonOptions);
                var props = new BasicProperties() {
                    Persistent = true,
                    MessageId = @event.EventId.ToString(),
                    Timestamp = new AmqpTimestamp(new DateTimeOffset(@event.OccurredOn).ToUnixTimeSeconds()),
                    Type = @event.EventType,
                    ContentType = "application/json",
                    Headers = new Dictionary<string, object?> {
                        ["service"] = @event.SourceService,
                        ["event-type"] = @event.EventType
                    }
                };

                await _channel.BasicPublishAsync(
                    exchange: exchange,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: props,
                    body: new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(jsonEvent)),
                    cancellationToken: ct
                );

                return true;

            } catch (Exception ex) {
                Console.WriteLine($"Failed to publish event {typeof(T).Name}: {ex.Message}");
                return false;
            }
        }

        private async Task EnsureExchangeExistsAsync(string exchange, CancellationToken ct) {
            if (!_declaredExchanges.Contains(exchange)) {
                await _channel.ExchangeDeclareAsync(
                    exchange: exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: ct
                );
                _declaredExchanges.Add(exchange);
            }
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
    }
}
