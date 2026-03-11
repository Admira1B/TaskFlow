using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Shared.Middlewares;
using TaskFlow.Shared.Core.Health;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Consul.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.ApiClients.Extensions;
using TaskFlow.Shared.ApiClients.IdentityService;
using TaskFlow.Tasks.API.Health;
using TaskFlow.Tasks.Application.Mapping;
using TaskFlow.Tasks.Application.Commands.Project.CreateProject;
using TaskFlow.Tasks.Infrastructure.Messaging;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Tasks.Infrastructure.SqlServer.Repositories;
using TaskFlow.Shared.Messaging.RabbitMQ.Health;
using TaskFlow.Shared.Messaging.RabbitMQ.Extensions;

namespace TaskFlow.Tasks.API.Composition {
    internal static class TasksServiceComposition {
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

            // === Controllers === 
            builder.Services.AddControllers();

            // === Infrastructure ===
            builder.Services.AddLogging(builder);
            builder.Services.AddSwaggerDocumentation(builder);
            builder.Services.AddJwtAuthentication(builder);
            builder.Services.AddConsulConfiguration(builder);
            builder.Services.AddConsulClient(builder);
            builder.Services.AddMediator(typeof(CreateProjectCommandHandler).Assembly);
            builder.Services.AddAutoMapper(typeof(TasksServiceMapperProfile).Assembly);
            builder.Services.AddRabbitMqEventConsumer<TasksServiceEventConsumer>();

            // === Database & Repositories ===
            builder.Services.AddDbContextWithMigrations<TasksServiceDbContext>(builder);
            builder.Services.AddRepositoriesFromAssembly(typeof(ProjectRepository).Assembly);

            // === Health Checks ===
            builder.Services.AddScoped<ConsulHealthCheck>();
            builder.Services.AddScoped<RabbitMqHealthCheck>();
            builder.Services.AddScoped<DataBaseHealthCheck<TasksServiceDbContext>>();
            builder.Services.AddHealthChecks()
                .AddCheck<TasksServiceHealthCheck>(nameof(TasksServiceHealthCheck), HealthStatus.Unhealthy);

            // === HttpClients ===
            builder.Services.AddServiceHttpClient<IdentityServiceClient>(builder.Configuration);

            return builder;
        }
    }
}
