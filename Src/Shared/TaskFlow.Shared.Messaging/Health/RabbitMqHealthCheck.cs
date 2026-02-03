using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskFlow.Shared.Messaging.Options;

namespace TaskFlow.Shared.Messaging.Health {
    public class RabbitMqHealthCheck(IOptions<RabbitMqOptions> options) : IHealthCheck {
        private readonly RabbitMqOptions _options = options.Value;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default) {
            try {
                var factory = new ConnectionFactory {
                    HostName = _options.HostName,
                    Port = _options.PortParsed,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    VirtualHost = _options.VirtualHost,
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(5),
                    SocketReadTimeout = TimeSpan.FromSeconds(5),
                    SocketWriteTimeout = TimeSpan.FromSeconds(5)
                };

                using var connection = await factory.CreateConnectionAsync(cancellationToken: ct);
                using var channel = await connection.CreateChannelAsync(cancellationToken: ct);

                if (!connection.IsOpen) {
                    return HealthCheckResult.Unhealthy(
                        description: "RabbitMQ connection is not open",
                        data: new Dictionary<string, object> {
                            ["host"] = _options.HostName,
                            ["port"] = _options.Port,
                            ["virtual_host"] = _options.VirtualHost
                        }
                    );
                }

                return HealthCheckResult.Healthy();
            } catch (Exception ex) {
                return HealthCheckResult.Unhealthy(
                    description: $"RabbitMQ health check failed: {ex.Message}",
                    exception: ex,
                    data: new Dictionary<string, object> {
                        ["rabbitmq_host"] = _options?.HostName ?? "unknown"
                    }
                );
            }
        }
    }
}
