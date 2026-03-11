using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Middlewares;
using TaskFlow.Shared.Core.Health;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Consul.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Identity.API.Health;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Application.Health;
using TaskFlow.Identity.Application.Mapping;
using TaskFlow.Identity.Application.Options;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Application.Commands.Auth.Register;
using TaskFlow.Identity.Infrastructure.Messaging;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Identity.Infrastructure.SqlServer.Repositories;
using TaskFlow.Shared.Messaging.RabbitMQ.Health;
using TaskFlow.Shared.Messaging.RabbitMQ.Extensions;

namespace TaskFlow.Identity.API.Composition {
    internal static class IdentityServiceComposition {
        public static WebApplication ConfigurePipeline(this WebApplication app) {
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.MapHealthChecks("/health");

            if (app.Environment.IsDevelopment()) {
                app.UseServiceSwaggerDocumentation();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
            // === Options ===
            builder.Services.AddConsulOptions(builder.Configuration);
            builder.Services.AddServiceOptions(builder.Configuration);
            builder.Services.AddRabbitMqOptions(builder.Configuration);
            builder.Services.AddJsonWebTokenOptions(builder.Configuration);
            builder.Services.AddJsonWebTokenGenerationOptions(builder.Configuration);

            // === Controllers === 
            builder.Services.AddControllers();

            // === Infrastructure ===
            builder.Services.AddLogging(builder);
            builder.Services.AddSwaggerDocumentation(builder);
            builder.Services.AddJwtAuthentication(builder);
            builder.Services.AddConsulConfiguration(builder);
            builder.Services.AddConsulClient(builder);
            builder.Services.AddMediator(typeof(RegisterCommandHandler).Assembly);
            builder.Services.AddAutoMapper(typeof(IdentityServiceMapperProfile).Assembly);
            builder.Services.AddRabbitMqEventPublisher<IEventPublisher, IdentityServiceEventPublisher>();
            builder.Services.AddAspNetIdentity<IdentityServiceDbContext, User, Role>();
            
            // === Database & Repositories ===
            builder.Services.AddDbContextWithMigrations<IdentityServiceDbContext>(builder);
            builder.Services.AddRepositoriesFromAssembly(typeof(UserRepository).Assembly);

            // === Health Checks ===
            builder.Services.AddScoped<ConsulHealthCheck>();
            builder.Services.AddScoped<IdentityHealthCheck>();
            builder.Services.AddScoped<RabbitMqHealthCheck>();
            builder.Services.AddScoped<DataBaseHealthCheck<IdentityServiceDbContext>>();
            builder.Services.AddHealthChecks()
                .AddCheck<IdentityServiceHealthCheck>(nameof(IdentityServiceHealthCheck), HealthStatus.Unhealthy);

            // === Services ===
            builder.Services.AddScoped<JsonWebTokenService>();

            return builder;
        }
    }
}
