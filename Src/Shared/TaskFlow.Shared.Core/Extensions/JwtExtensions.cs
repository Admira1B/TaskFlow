using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Shared.Core.Options;
using Microsoft.AspNetCore.Builder;

namespace TaskFlow.Shared.Core.Extensions {
    public static class JwtExtensions {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder) {
            var jwtOptions = builder.Configuration.GetSection(nameof(JsonWebTokenOptions)).Get<JsonWebTokenOptions>()
                ?? throw new InvalidOperationException("JsonWebTokenOptions not configured");

            services
                .AddAuthentication(options => {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    ConfigureJwtBearer(options, jwtOptions);
                });

            services.AddAuthorization();

            return services;
        }

        private static void ConfigureJwtBearer(JwtBearerOptions jwtBearerOptions, JsonWebTokenOptions jwtOptions) {
            jwtBearerOptions.SaveToken = true;
            jwtBearerOptions.RequireHttpsMetadata = false;

            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
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

            jwtBearerOptions.Events = new JwtBearerEvents {
                OnMessageReceived = context => {
                    var authHeader = context.Request.Headers["Authorization"].ToString();
                    if (authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true) {
                        context.Token = authHeader["Bearer ".Length..].Trim();
                    }
                    return Task.CompletedTask;
                }
            };
        }
    }
}
