using Ocelot.Provider.Consul;
using Ocelot.DependencyInjection;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Consul;
using TaskFlow.Shared.Consul.Options;

namespace TaskFlow.Gateway.ServicesRouting {
    internal static class ServicesRoutingExtensions {
        public static IServiceCollection AddOcelotRoutingWithConsulSupport(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetServiceOptions();
            var consulOptions = builder.Configuration.GetConsulOptions();

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment, MergeOcelotJson.ToMemory)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string?> {
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Host"] = consulOptions.Host,
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Port"] = consulOptions.Port.ToString(),
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Type"] = "Consul",
                    ["GlobalConfiguration:BaseUrl"] = serviceOptions.Address
                });

            services.AddOcelot(builder.Configuration).AddConsul<OcelotServiceConsulProviderBuilder>();

            return services;
        }
    }
}
