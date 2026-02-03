using System.Text;
using System.Text.Json;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Core.Entities;

namespace TaskFlow.Shared.Messaging {
    public abstract class RabbitMqEventConsumer : BackgroundService {
        private readonly ILogger _logger;
        private readonly RabbitMqOptions _options;
        private readonly IServiceProvider _services;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Dictionary<string, string> _subscriptions;

        private bool _disposed = false;
        private IChannel? _channel;
        private IConnection? _connection;

        public RabbitMqEventConsumer(ILogger logger, IOptions<RabbitMqOptions> options, IServiceProvider services, Dictionary<string, string> subscriptions) {
            _logger = logger;
            _options = options.Value;
            _services = services;
            _subscriptions = subscriptions;

            _jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected override async Task ExecuteAsync(CancellationToken ct) {
            _logger.Info($"{GetType().Name} starting...");

            try {
                await ConnectAsync(ct);

                await SetupSubscriptionsAsync(ct);

                _logger.Info($"Subscribed to {_subscriptions.Count} exchange(s)");

                _logger.Info($"{GetType().Name} started successfully");

                while (!ct.IsCancellationRequested) {
                    await Task.Delay(1000, ct);
                }
            } catch (OperationCanceledException) {
                _logger.Info($"{GetType().Name} stopped gracefully");
            } catch (Exception ex) {
                _logger.Fatal($"{this.GetType().Name} stopped with critical failure", ex);
                throw;
            } finally {
                await CleanupAsync();
            }
        }

        private async Task ConnectAsync(CancellationToken ct) {
            try {
                var factory = new ConnectionFactory {
                    UserName = _options.UserName,
                    Password = _options.Password,
                    Port = _options.PortParsed,
                    HostName = _options.HostName,
                    VirtualHost = _options.VirtualHost,
                    AutomaticRecoveryEnabled = true

                };

                _connection = await factory.CreateConnectionAsync(cancellationToken: ct);
                _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

                await _channel.BasicQosAsync(
                    prefetchSize: 0,
                    prefetchCount: 10,
                    global: false,
                    cancellationToken: ct
                );

                _logger.Debug($"RabbitMQ channel created");
            } catch (OperationCanceledException) {
                _logger.Warn("RabbitMQ connection cancelled");
                throw;
            } catch (Exception ex) {
                _logger.Fatal($"Failed to connect to RabbitMQ at {_options.HostName}:{_options.Port}", ex);
                throw;
            }

        }

        private async Task SetupSubscriptionsAsync(CancellationToken ct) {
            foreach (var (exchangeName, routingPattern) in _subscriptions) {
                await SetupSubscriptionAsync(exchangeName, routingPattern, ct);
            }
        }

        private async Task SetupSubscriptionAsync(string exchangeName, string routingPattern, CancellationToken ct) {
            var queueName = $"tasks-service.{exchangeName}";

            await _channel!.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct
            );

            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct
            );

            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingPattern,
                arguments: null,
                cancellationToken: ct
            );

            await StartConsumingQueueAsync(queueName, ct);
        }

        private async Task StartConsumingQueueAsync(string queueName, CancellationToken ct) {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) => {
                try {
                    await ProcessMessageAsync(ea, ct);
                    await _channel!.BasicAckAsync(ea.DeliveryTag, false, ct);

                    _logger.Debug($"Message {ea.DeliveryTag} successfully processed.");
                } catch (JsonException jsonEx) {
                    _logger.Error($"Invalid message format in queue '{queueName}' - delivery tag {ea.DeliveryTag}", jsonEx);
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
                } catch (InvalidOperationException invalidOpEx) {
                    _logger.Error($"Business logic violation while processing message {ea.DeliveryTag} from '{queueName}'", invalidOpEx);
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
                } catch (Exception ex) {
                    _logger.Error($"Failed to process message {ea.DeliveryTag} from queue '{queueName}'", ex);
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, true, ct);
                }
            };

            await _channel!.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: ct
            );
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken ct) {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var eventTypeName = ea.BasicProperties.Type;

            _logger.Debug($"Processing message: {ea.BasicProperties.Type} -> {ea.RoutingKey} (delivery tag: {ea.DeliveryTag})");

            if (eventTypeName is null) {
                throw new InvalidOperationException($"Message missing event type - delivery tag {ea.DeliveryTag}, routing key '{ea.RoutingKey}'");
            }

            var eventType = FindEventType(eventTypeName);
            if (eventType == null) {
                throw new InvalidOperationException($"Unsupported event type '{eventTypeName}' - delivery tag {ea.DeliveryTag}");
            }

            var integrationEvent = JsonSerializer.Deserialize(messageJson, eventType, _jsonOptions);
            if (integrationEvent == null) {
                throw new InvalidOperationException($"Failed to deserialize event '{eventTypeName}' - delivery tag {ea.DeliveryTag}");
            }

            using var scope = _services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(integrationEvent, ct);

            _logger.Debug($"Event {eventTypeName} published to mediator");
        }

        private Type? FindEventType(string eventTypeName) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Name == eventTypeName &&
                    typeof(EventBase).IsAssignableFrom(t)
                );
        }

        private async Task CleanupAsync() {
            if (_channel?.IsOpen == true) {
                await _channel.CloseAsync();
            }

            if (_connection?.IsOpen == true) {
                await _connection.CloseAsync();
            }

            _channel?.Dispose();
            _connection?.Dispose();
        }

        public override async Task StopAsync(CancellationToken ct) {
            await CleanupAsync();
            await base.StopAsync(ct);
        }

        public override void Dispose() {
            if (!_disposed) {
                CleanupAsync().GetAwaiter().GetResult();
                base.Dispose();
                _disposed = true;
            }
        }
    }
}
