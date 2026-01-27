using System.Text;
using NLog;
using NLog.Web;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskFlow.Gateway.Health;
using TaskFlow.Gateway.Options;

namespace TaskFlow.Gateway.Composition {
    internal static class GatewayComposition {
        public async static Task<WebApplication> ConfigurePipelineAsync(this WebApplication app) {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions {
                    Predicate = check => check.Tags.Contains("ready")
                });
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions {
                    Predicate = check => check.Tags.Contains("live")
                });
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
            // Nlog logger
            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<Shared.Core.Interfaces.ILogger>(provider => {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

                var nlogLogger = LogManager.GetLogger("gateway");

                return new Shared.Logging.Logger(nlogLogger, contextAccessor);
            });

            // Health Checks
            builder.Services.AddHealthChecks()
                .AddCheck<GatewayHealthCheck>("gateway_health_check",
                    HealthStatus.Unhealthy,
                    new[] { "ready", "live" });

            // Ocelot
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment)
                .AddEnvironmentVariables();

            builder.Services.AddOcelot(builder.Configuration);

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
