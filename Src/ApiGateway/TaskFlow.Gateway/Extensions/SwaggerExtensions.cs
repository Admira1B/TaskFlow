namespace TaskFlow.Gateway.Extensions {
    public static class SwaggerExtensions {
        public static IServiceCollection AddOcelotSwaggerDocumentation(this IServiceCollection services, WebApplicationBuilder builder) {
            services.AddSwaggerForOcelot(builder.Configuration);

            return services;
        }

        public static WebApplication UseOcelotSwaggerDocumentation(this WebApplication app) {
            app.UseSwaggerForOcelotUI(ui => {
                ui.PathToSwaggerGenerator = "/swagger/docs";
            });

            return app;
        }
    }
}
