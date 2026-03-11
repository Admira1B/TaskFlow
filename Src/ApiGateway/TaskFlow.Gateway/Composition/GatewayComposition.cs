using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ocelot.Middleware;
using TaskFlow.Shared.Middlewares;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Consul.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Gateway.Health;
using TaskFlow.Gateway.Extensions;

namespace TaskFlow.Gateway.Composition {
    internal static class GatewayComposition {
        public async static Task<WebApplication> ConfigurePipelineAsync(this WebApplication app) {
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health");
            });

            if (app.Environment.IsDevelopment()) {
                app.UseOcelotSwaggerDocumentation();
            }

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
            builder.Services.AddOcelotRoutingWithConsulSupport(builder);
            builder.Services.AddOcelotSwaggerDocumentation(builder);

            // === Health Checks ===
            builder.Services.AddHealthChecks()
                .AddCheck<GatewayHealthCheck>(nameof(GatewayHealthCheck), HealthStatus.Unhealthy);

            return builder.Services;
        }
    }
}
