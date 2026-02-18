using System.Text;
using NLog;
using NLog.Web;
using Consul;
using Winton.Extensions.Configuration.Consul;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Shared.Consul;
using TaskFlow.Shared.Consul.Health;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.Messaging.Health;
using TaskFlow.Shared.Messaging.Options;
using TaskFlow.Identity.API.Health;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts;
using TaskFlow.Identity.Application.Health;
using TaskFlow.Identity.Application.Mapping;
using TaskFlow.Identity.Application.Options;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Infrastructure.Messaging;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Identity.Infrastructure.SqlServer.Health;
using TaskFlow.Identity.Infrastructure.SqlServer.Repositories;
using TaskFlow.Shared.Middlewares;

namespace TaskFlow.Identity.API.Composition {
    internal static class IdentityServiceComposition {
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
                var nlogLogger = LogManager.GetLogger("identity-service");
                return new Shared.Logging.Logger(nlogLogger, contextAccessor);
            });

            // DbContext
            builder.Services.AddDbContext<IdentityServiceDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(IdentityServiceDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            // Data Access
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            // Health Checks
            builder.Services.AddScoped<IdentityHealthCheck>();
            builder.Services.AddScoped<DataBaseHealthCheck>();
            builder.Services.AddScoped<RabbitMqHealthCheck>();
            builder.Services.AddScoped<ConsulHealthCheck>();
            builder.Services.AddHealthChecks()
                .AddCheck<ServiceHealthCheck>("identity_service_health_check", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);

            // Consul 
            builder.Services.Configure<ConsulOptions>(builder.Configuration.GetSection(nameof(ConsulOptions)));
            builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection(nameof(ServiceOptions)));

            builder.Configuration.AddConsul($"config/tasks/{builder.Environment.EnvironmentName}", options => {
                options.ConsulConfigurationOptions = cc => {
                    cc.Address = new Uri(builder.Configuration["ConsulOptions:Address"] ??
                        throw new InvalidOperationException(
                            "ConsulOptions:Address configuration is missing. Check 'ConsulOptions:Address' into appsettings.json or environment variables."
                        )
                    );
                };
                options.Optional = true;
                options.ReloadOnChange = true;
                options.PollWaitTime = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddSingleton<IConsulClient>(sp => {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new ConsulClient(c => {
                    c.Datacenter = configuration["ConsulOptions:Datacenter"] ??
                        throw new InvalidOperationException(
                            "ConsulOptions:Datacenter configuration is missing. Check 'ConsulOptions:Datacenter' into appsettings.json or environment variables."
                        );
                    c.Address = new Uri(configuration["ConsulOptions:Address"] ??
                        throw new InvalidOperationException(
                            "ConsulOptions:Address configuration is missing. Check 'ConsulOptions:Address' into appsettings.json or environment variables."
                        )
                    );
                });
            });
            builder.Services.AddHostedService<ConsulHostedService>();

            // Swagger Documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new() {
                    Version = "v1",
                    Title = "TaskFlow Identity Service",
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
            builder.Services.AddSingleton<IEventPublisher, IdentityServiceEventPublisher>();
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));

            // MediatoR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(IdentityServiceMapperProfile).Assembly);

            // ASP Identity
            builder.Services.AddIdentity<User, Domain.Entities.Role>(options => {
                // Password Options
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                // User Options
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityServiceDbContext>()
            .AddDefaultTokenProviders();

            // JsonWebToken Authentication & Authorization
            builder.Services.AddScoped<JsonWebTokenService>();
            builder.Services.Configure<JsonWebTokenGenerationOptions>(builder.Configuration.GetSection(nameof(JsonWebTokenGenerationOptions)));

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
