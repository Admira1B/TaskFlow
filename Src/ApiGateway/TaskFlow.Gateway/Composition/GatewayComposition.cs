using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.Middleware;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Consul.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Gateway.Health;
using TaskFlow.Shared.Middlewares;

namespace TaskFlow.Gateway.Composition {
    internal static class GatewayComposition {
        public async static Task<WebApplication> ConfigurePipelineAsync(this WebApplication app) {
            app.UseMiddleware<RequestLoggingMiddleware>();
            
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            await app.UseOcelot();

            return app;
        }

        public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder) {
            // === Options ===
            builder.Services.AddConsulOptions(builder.Configuration);
            builder.Services.AddServiceOptions(builder.Configuration);
            builder.Services.AddJsonWebTokenOptions(builder.Configuration);

            // === Infrastructure ===
            builder.Services.AddLogging(builder);
            builder.Services.AddConsulClient(builder);
            builder.Services.AddJwtAuthentication(builder);
            builder.Services.AddGatewayOcelotWithConsulSupport(builder);

            // === Health Checks ===
            builder.Services.AddHealthChecks()
                .AddCheck<GatewayHealthCheck>(nameof(GatewayHealthCheck), HealthStatus.Unhealthy);

            return builder.Services;
        }
    }

    internal static class OcelotExtensions {
        public static IServiceCollection AddGatewayOcelotWithConsulSupport(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()
                ?? throw new InvalidOperationException("ServiceOptions not configured");
            var consulOptions = builder.Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>()
                ?? throw new InvalidOperationException("ConsulOptions not configured");

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment)
                .AddEnvironmentVariables();

            builder.Services.PostConfigure<FileConfiguration>(config => {
                config.GlobalConfiguration.ServiceDiscoveryProvider ??= new FileServiceDiscoveryProvider();

                config.GlobalConfiguration.ServiceDiscoveryProvider.Host = consulOptions.Host;
                config.GlobalConfiguration.ServiceDiscoveryProvider.Port = consulOptions.Port;
                config.GlobalConfiguration.ServiceDiscoveryProvider.Type = "Consul";

                config.GlobalConfiguration.BaseUrl = $"http://{serviceOptions.Host}:{serviceOptions.Port}";
            });

            builder.Services.AddOcelot(builder.Configuration).AddConsul().AddConfigPlaceholders();

            return services;
        }
    }
}
