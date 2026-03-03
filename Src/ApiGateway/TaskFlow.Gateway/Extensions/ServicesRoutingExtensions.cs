using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using TaskFlow.Shared.Consul;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Core.Helpers;
using TaskFlow.Shared.Core.Options;

namespace TaskFlow.Gateway.Extensions {
    internal static class ServicesRoutingExtensions {
        public static IServiceCollection AddOcelotRoutingWithConsulSupport(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetServiceOptions();
            var consulOptions = builder.Configuration.GetConsulOptions();
            var applicationVersion = ApplicationHelper.GetMajorVersion();

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment, MergeOcelotJson.ToMemory)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string?> {
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Host"] = consulOptions.Host,
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Port"] = consulOptions.Port.ToString(),
                    ["GlobalConfiguration:ServiceDiscoveryProvider:Type"] = "Consul",
                    ["GlobalConfiguration:BaseUrl"] = serviceOptions.Address,

                    ["SwaggerEndPoints:0:Config:0:Version"] = "v0",
                    ["SwaggerEndPoints:0:Config:0:Service:Path"] = $"/swagger/v0/swagger.json"
                });

            services.AddOcelot(builder.Configuration).AddConsul<OcelotServiceConsulProviderBuilder>();

            return services;
        }
    }
}
