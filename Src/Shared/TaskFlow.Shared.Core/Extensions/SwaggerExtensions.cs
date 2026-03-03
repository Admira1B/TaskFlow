using Microsoft.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Helpers;

namespace TaskFlow.Shared.Core.Extensions {
    public static class SwaggerExtensions {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetServiceOptions();
            var applicationVersion = ApplicationHelper.GetMajorVersion();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc(applicationVersion, new() {
                    Version = applicationVersion,
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
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            return services;
        }

        public static WebApplication UseServiceSwaggerDocumentation(this WebApplication app) {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
