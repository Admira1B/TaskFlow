using System.Text;
using System.Text.Json;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Messaging.Events;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.Messaging.Constants;

namespace TaskFlow.Tasks.Infrastructure.Messaging {
    public class EventConsumer : BackgroundService {
        private readonly RabbitMqOptions _options;
        private readonly IServiceProvider _services;
        private readonly JsonSerializerOptions _jsonOptions;

        private bool _disposed = false;
        private IChannel? _channel;
        private IConnection? _connection;

        public EventConsumer(IOptions<RabbitMqOptions> options, IServiceProvider services) {
            _options = options.Value;
            _services = services;

            _jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected override async Task ExecuteAsync(CancellationToken ct) {
            Console.WriteLine("EventConsumer starting...");

            try {
                await ConnectAsync(ct);

                await SetupSubscriptionsAsync(ct);

                while (!ct.IsCancellationRequested) {
                    await Task.Delay(1000, ct);
                }
            } catch (OperationCanceledException) {
                Console.WriteLine("EventConsumer stopped gracefully");
            } catch (Exception ex) {
                Console.WriteLine($"Critical error in EventConsumer: {ex.Message}");
                throw;
            } finally {
                await CleanupAsync();
            }
        }

        private async Task ConnectAsync(CancellationToken ct) {
            var factory = new ConnectionFactory {
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
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

            Console.WriteLine("Connected to RabbitMQ");
        }

        private async Task SetupSubscriptionsAsync(CancellationToken ct) {
            var subscriptions = new Dictionary<string, string> {
                // Subscriptions adding
                [RabbitMqConstants.IdentityService.ExchangeName] = RabbitMqConstants.IdentityService.RoutingPattern,
            };

            foreach (var (exchangeName, routingPattern) in subscriptions) {
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

            Console.WriteLine($"Subscribed to {exchangeName} -> {queueName} ({routingPattern})");
        }

        private async Task StartConsumingQueueAsync(string queueName, CancellationToken ct) {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) => {
                try {
                    await ProcessMessageAsync(ea, ct);
                    await _channel!.BasicAckAsync(ea.DeliveryTag, false, ct);
                } catch (JsonException jsonEx) {
                    Console.WriteLine($"JSON error: {jsonEx.Message}");
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
                } catch (InvalidOperationException) {
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false, ct);
                } catch (Exception ex) {
                    Console.WriteLine($"Error processing message: {ex.Message}");

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

            if (eventTypeName is null) {
                throw new NullReferenceException("Message have no event type");
            }

            Console.WriteLine($"Received: {eventTypeName} -> {ea.RoutingKey}");

            var eventType = FindEventType(eventTypeName);
            if (eventType == null) {
                throw new InvalidOperationException($"Unknown event type: {eventTypeName}");
            }

            var integrationEvent = JsonSerializer.Deserialize(messageJson, eventType, _jsonOptions);
            if (integrationEvent == null) {
                throw new InvalidOperationException($"Failed to deserialize {eventTypeName}");
            }

            using var scope = _services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(integrationEvent, ct);

            Console.WriteLine($"Processed: {eventTypeName}");
        }

        private Type? FindEventType(string eventTypeName) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Name == eventTypeName &&
                    typeof(BaseEvent).IsAssignableFrom(t)
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

            //Console.WriteLine("EventConsumer cleaned up");
        }

        public override async Task StopAsync(CancellationToken ct) {
            //Console.WriteLine("EventConsumer stopping...");
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
