using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using TaskFlow.Shared.Core.Options;

namespace TaskFlow.Shared.Core.Extensions {
    public static class SwaggerExtensions {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, WebApplicationBuilder builder, string version = "v1") {
            var serviceOptions = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()
                ?? throw new InvalidOperationException("ServiceOptions not configured");

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc(version, new() {
                    Version = version,
                    Title = $"TaskFlow {serviceOptions.Name}",
                    Contact = new() {
                        Name = "Vlad Reizenbuk",
                        Email = "vreizenbuk@mail.ru"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app) {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
