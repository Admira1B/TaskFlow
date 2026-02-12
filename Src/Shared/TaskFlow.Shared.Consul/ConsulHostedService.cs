using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Consul.Options;

namespace TaskFlow.Shared.Consul {
    public class ConsulHostedService(ILogger logger, IOptions<ConsulOptions> consulOptions,IOptions<ServiceOptions> serviceOptions, IConsulClient consulClient) : IHostedService {
        private readonly ILogger _logger = logger;
        private readonly ConsulOptions _consulOptions = consulOptions.Value;
        private readonly ServiceOptions _serviceOptions = serviceOptions.Value;
        private readonly IConsulClient _consulClient = consulClient;
        private string _serviceId = string.Empty;

        public async Task StartAsync(CancellationToken cancellationToken) {
            if (!_consulOptions.EnableServiceDiscoveryParsed) {
                _logger.Debug("Service discovery is disabled");
                return;
            }

            try {
                _serviceId = $"{_serviceOptions.Name}-{Guid.NewGuid():N}";

                var registration = new AgentServiceRegistration {
                    ID = _serviceId,
                    Name = _serviceOptions.Name,
                    Address = _serviceOptions.Host,
                    Port = _serviceOptions.PortParsed,
                    Check = new AgentServiceCheck {
                        HTTP = $"http://{_serviceOptions.Host}:{_serviceOptions.PortParsed}/health",
                        Interval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(5),
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                    }
                };

                await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
                await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

                _logger.Info($"Service '{_serviceOptions.Name}' registered with Consul");
            } catch (Exception ex) {
                _logger.Error($"Failed to register service '{_serviceOptions.Name}' with Consul", ex);
                throw;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken) {
            if (string.IsNullOrEmpty(_serviceId) || !_consulOptions.EnableServiceDiscoveryParsed)
                return;

            try {
                await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
                _logger.Info($"Service '{_serviceOptions.Name}' removed from Consul");
            } catch (Exception ex) {
                _logger.Error($"Failed to remove service '{_serviceOptions.Name}' from Consul", ex);
            }
        }
    }
}
