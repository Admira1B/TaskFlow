using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TaskFlow.Shared.Messaging.Options;

namespace TaskFlow.Tasks.Infrastructure.Messaging {
    public class EventConsumer : BackgroundService {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMqOptions _options;
        private readonly IServiceProvider _services;
        private bool _disposed = false;

        public EventConsumer(IOptions<RabbitMqOptions> options, IServiceProvider serviceProvider) {
            _options = options.Value;
            _services = serviceProvider;


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
        
        protected override async Task ExecuteAsync(CancellationToken ct = default) {

        }
    }
}
