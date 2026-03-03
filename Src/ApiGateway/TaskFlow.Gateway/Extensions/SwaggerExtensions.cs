using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;

namespace TaskFlow.Gateway.Extensions {
    public static class SwaggerExtensions {
        public static IServiceCollection AddOcelotSwaggerDocumentation(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetServiceOptions();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerForOcelot(builder.Configuration);

            services.AddSwaggerDocumentation(builder);

            return services;
        }

        public static WebApplication UseOcelotSwaggerDocumentation(this WebApplication app) {
            app.UseSwagger();
            app.UseSwaggerForOcelotUI();

            return app;
        }
    }
}
