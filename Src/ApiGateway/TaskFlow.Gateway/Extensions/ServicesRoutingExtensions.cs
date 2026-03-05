using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using TaskFlow.Shared.Consul;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Helpers;

namespace TaskFlow.Gateway.Extensions {
    internal static class ServicesRoutingExtensions {
        public static IServiceCollection AddOcelotRoutingWithConsulSupport(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetServiceOptions();
            var consulOptions = builder.Configuration.GetConsulOptions();

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("Ocelot/Swagger/ocelot.endpoints.json", optional: false, reloadOnChange: false)
                .AddOcelot("Ocelot", builder.Environment, MergeOcelotJson.ToMemory)
                .AddEnvironmentVariables();

            builder.Configuration.AddInMemoryCollection(
                builder.Configuration.BuildDynamicOcelotConfiguration(serviceOptions, consulOptions)
            );

            services.AddOcelot(builder.Configuration).AddConsul<OcelotServiceConsulProviderBuilder>();

            return services;
        }

        private static Dictionary<string, string?> BuildDynamicOcelotConfiguration(this IConfiguration configuration, ServiceOptions serviceOptions, ConsulOptions consulOptions) {
            var version = ApplicationHelper.GetMajorVersion();
            var dynamicConfiguration = new Dictionary<string, string?> {
                // Global configuration
                ["GlobalConfiguration:ServiceDiscoveryProvider:Host"] = consulOptions.Host,
                ["GlobalConfiguration:ServiceDiscoveryProvider:Port"] = consulOptions.Port.ToString(),
                ["GlobalConfiguration:ServiceDiscoveryProvider:Type"] = "Consul",
                ["GlobalConfiguration:BaseUrl"] = serviceOptions.Address,
            };

            var swaggerEndPoints = configuration.GetSection("SwaggerEndPoints").GetChildren().ToList();

            for (int counter = 0; counter < swaggerEndPoints.Count; counter++) {
                var key = swaggerEndPoints[counter].GetValue<string>("Key");
                if (string.IsNullOrEmpty(key)) continue;

                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Key"] = key;

                dynamicConfiguration[$"SwaggerEndPoints:{counter}:TakeServersFromDownstreamService"] = "true";

                var currentName = swaggerEndPoints[counter]
                    .GetSection("Config")
                    .GetChildren()
                    .FirstOrDefault()?["Name"];

                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Config:0:Name"] = currentName ?? key.KeyToName();
                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Config:0:Version"] = version;
                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Config:0:Service:Name"] = key;
                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Config:0:Service:Path"] = $"/swagger/{version}/swagger.json";
                dynamicConfiguration[$"SwaggerEndPoints:{counter}:Config:0:Description"] = $"{key.KeyToName()} - {version}";
            }

            return dynamicConfiguration;
        }

        private static string KeyToName(this string key) {
            return string.Join(" ", key.Split('-')
                .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1))) + " API";
        }
    }
}
