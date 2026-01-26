using System.Text;
using NLog;
using NLog.Web;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Gateway.Options;

namespace TaskFlow.Gateway.Composition {
    internal static class GatewayComposition {
        public async static Task<WebApplication> ConfigurePipelineAsync(this WebApplication app) {

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
            // Logging
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

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

            // Ocelot
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment)
                .AddEnvironmentVariables();

            builder.Services.AddOcelot(builder.Configuration);

            return builder.Services;
        }
    }
}
