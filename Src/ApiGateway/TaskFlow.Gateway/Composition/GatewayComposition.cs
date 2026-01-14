using System.Text;
using Ocelot.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Gateway.Options;

namespace TaskFlow.Gateway.Composition {
    public static class GatewayComposition {
        public static IServiceCollection AddGatewayComposition(this WebApplicationBuilder builder) {
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
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

            builder.Services.AddAuthorization();

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot("OcelotConfigurations", builder.Environment)
                .AddEnvironmentVariables();

            builder.Services.AddOcelot(builder.Configuration);

            return builder.Services;
        }
    }
}
