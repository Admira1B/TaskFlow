using System.Text;
using NLog;
using NLog.Web;
using Consul;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Gateway.Health;
using TaskFlow.Shared.Consul;
using TaskFlow.Shared.Consul.Options;
using TaskFlow.Shared.Core.Options;

namespace TaskFlow.Gateway.Composition {
    internal static class GatewayComposition {
        public async static Task<WebApplication> ConfigurePipelineAsync(this WebApplication app) {
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) => {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Info($"Request: {context.Request.Method} {context.Request.Path}");
                await next();
                logger.Info($"Response: {context.Response.StatusCode}");
            });

            await app.UseOcelot();

            return app;
        }

        public static IServiceCollection ConfigureServices(this WebApplicationBuilder builder) {
            // Health Checks
            builder.Services.AddHealthChecks()
                .AddCheck<GatewayHealthCheck>("gateway_health_check", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);
            
            // Nlog logger
            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<Shared.Core.Interfaces.ILogger>(provider => {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                var nlogLogger = LogManager.GetLogger("gateway");

                return new Shared.Logging.Logger(nlogLogger, contextAccessor);
            });

            // Services routing (Consul & Ocelot)
            builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection(nameof(ServiceOptions)));
            builder.Services.Configure<ConsulOptions>(builder.Configuration.GetSection(nameof(ConsulOptions)));

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

            var serviceOpts = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()!;
            var consulOpts = builder.Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>()!;

            var consulUri = new Uri(consulOpts.Address);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment)
                .AddEnvironmentVariables();

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> {
                ["GlobalConfiguration:BaseUrl"] = $"http://{serviceOpts.Host}:{serviceOpts.Port}",
                ["GlobalConfiguration:ServiceDiscoveryProvider:Type"] = "Consul",
                ["GlobalConfiguration:ServiceDiscoveryProvider:Host"] = consulUri.Host,
                ["GlobalConfiguration:ServiceDiscoveryProvider:Port"] = consulUri.Port.ToString()
            });

            builder.Services.AddOcelot(builder.Configuration).AddConsul();

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
            });

            builder.Services.AddAuthorization();

            return builder.Services;
        }
    }
}
