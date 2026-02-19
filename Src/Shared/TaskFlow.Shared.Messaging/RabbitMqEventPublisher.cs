using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Core.Abstractions;

namespace TaskFlow.Shared.Messaging {
    public abstract class RabbitMqEventPublisher : IDisposable {
        private readonly ILogger _logger;
        private readonly IChannel _channel;
        private readonly IConnection _connection;
        private readonly RabbitMqOptions _options;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly HashSet<string> _declaredExchanges;

        private bool _disposed = false;

        protected RabbitMqEventPublisher(ILogger logger, IOptions<RabbitMqOptions> options) {
            _logger = logger;
            _options = options.Value;
            _jsonOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            _declaredExchanges = [];

            var factory = new ConnectionFactory {
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                HostName = _options.Host,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        protected async Task<bool> PublishEventAsync<T>(T @event, string exchange, string routingKey, string serviceName, CancellationToken cancellationToken = default) where T : EventBase {
            ArgumentNullException.ThrowIfNull(@event);

            try {
                await EnsureExchangeExistsAsync(exchange, cancellationToken);

                @event.SourceService = serviceName;
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
                    cancellationToken: cancellationToken
                );

                return true;

            } catch (Exception ex) {
                _logger.Error($"Failed to publish {typeof(T).Name} event to {exchange}/{routingKey}", ex);
                return false;
            }
        }

        private async Task EnsureExchangeExistsAsync(string exchange, CancellationToken cancellationToken) {
            if (!_declaredExchanges.Contains(exchange)) {
                await _channel.ExchangeDeclareAsync(
                    exchange: exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken
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

        ~RabbitMqEventPublisher() {
            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}
