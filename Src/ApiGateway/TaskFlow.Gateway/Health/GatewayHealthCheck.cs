using TaskFlow.Shared.Core.Abstractions;

namespace TaskFlow.Gateway.Health {
    public class GatewayHealthCheck(Shared.Core.Interfaces.ILogger logger) :
                 ServiceHealthCheckBase(logger, serviceName: "Gateway") {
    }
}
