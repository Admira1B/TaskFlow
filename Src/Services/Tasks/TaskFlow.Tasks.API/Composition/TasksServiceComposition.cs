using System.Text;
using NLog;
using NLog.Web;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Middlewares;
using TaskFlow.Shared.Messaging.Health;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Shared.ApiClients.IdentityService;
using TaskFlow.Tasks.API.Health;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Mapping;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Infrastructure.Messaging;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Tasks.Infrastructure.SqlServer.Health;
using TaskFlow.Tasks.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Tasks.API.Composition {
    internal static class TasksServiceComposition {
        public static WebApplication ConfigurePipeline(this WebApplication app) {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.MapHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder) {
            // Controllers
            builder.Services.AddControllers();

            // Nlog logger
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<Shared.Core.Interfaces.ILogger>(provider => {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var nlogLogger = LogManager.GetLogger("tasks-service");
                return new Shared.Logging.Logger(nlogLogger, contextAccessor);
            });

            // DbContext
            builder.Services.AddDbContext<TaskServiceDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(TaskServiceDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            // Data Access
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            builder.Services.AddScoped<ITaskGroupRepository, TaskGroupRepository>();
            builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            
            // Health Checks
            builder.Services.AddScoped<DataBaseHealthCheck>();
            builder.Services.AddScoped<RabbitMqHealthCheck>();
            builder.Services.AddHealthChecks()
                .AddCheck<ServiceHealthCheck>("tasks_service_health_check", HealthStatus.Unhealthy);

            // HttpClients 
            builder.Services.AddHttpClient<IdentityServiceClient>((services, client) => {
                var configuration = services.GetRequiredService<IConfiguration>();

                var uriBase = builder.Environment.EnvironmentName.ToLower() switch {
                    "docker" => "http://taskflow-gateway:8080/flow",
                    _ => "http://localhost:5001"
                };

                client.BaseAddress = new Uri(uriBase);

                client.DefaultRequestHeaders.Add("Accept", "application/json");

                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                ConnectTimeout = TimeSpan.FromSeconds(5),
                MaxConnectionsPerServer = 20
            });

            // Swagger Documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new() {
                    Version = "v1",
                    Title = "TaskFlow Tasks Service",
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

            // RabbitMQ
            builder.Services.AddHostedService<TasksServiceEventConsumer>();
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));

            // MediatoR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateCommentCommandHandler).Assembly));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(TaskServiceMapperProfile).Assembly);

            // JsonWebToken Authentication & Authorization
            var jwtOptions = builder.Configuration.GetSection(nameof(JsonWebTokenOptions)).Get<JsonWebTokenOptions>();

            builder.Services.AddAuthentication(options => {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidateAudience = true,
                    AudienceValidator = (audiences, token, validationParams) => {
                        if (audiences is null) {
                            return false;
                        }

                        return audiences.Contains(jwtOptions.Audience);
                    },
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents {
                    OnMessageReceived = context => {
                        var authHeader = context.Request.Headers["Authorization"].ToString();
                        if (authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true) {
                            context.Token = authHeader["Bearer ".Length..].Trim();
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization();

            return builder.Services;
        }
    }
}
