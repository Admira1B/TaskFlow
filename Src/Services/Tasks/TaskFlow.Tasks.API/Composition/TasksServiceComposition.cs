using System.Text;
using NLog;
using NLog.Web;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Middlewares;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Tasks.Application.Mapping;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Infrastructure.Messaging;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Tasks.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Tasks.API.Composition {
    internal static class TasksServiceComposition {
            public static WebApplication ConfigurePipeline(this WebApplication app) {
                app.UseMiddleware<RequestLoggingMiddleware>();

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

            // Swagger Documentation
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

            // Nlog logger
            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<Shared.Core.Interfaces.ILogger>(provider => {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                var nlogLogger = LogManager.GetLogger("tasks-service");

                return new Shared.Logging.Logger(nlogLogger, contextAccessor);
            });

            // RabbitMQ
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
            builder.Services.AddHostedService<TasksServiceEventConsumer>();

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
