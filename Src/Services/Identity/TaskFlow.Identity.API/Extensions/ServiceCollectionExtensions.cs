using System.Text;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Identity.Application.Mapping;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Domain.Contracts.Services;
using TaskFlow.Identity.Domain.Contracts.Repositories;
using TaskFlow.Identity.Domain.Options;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Identity.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Identity.API.Extensions {
    internal static class ServiceCollectionExtensions {
        public static IServiceCollection AddIdentityServiceDependencies(this IServiceCollection services, IConfiguration configuration) {
            // Adding controllers
            services.AddControllers();

            // Adding documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
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
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                options.AddSecurityRequirement(document => {
                    var schemeReference = new OpenApiSecuritySchemeReference("Bearer");

                    var requirement = new OpenApiSecurityRequirement {
                        [schemeReference] = []
                    };

                    return requirement;
                });
            });

            // DbContext
            services.AddDbContext<IdentityServiceDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(IdentityServiceDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            // Data Access
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // MediatoR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(IdentityServiceMapperProfile).Assembly);

            // ASP Identity
            services.AddIdentity<User, Role>(options => {
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
            services.AddScoped<IJsonWebTokenService, JsonWebTokenService>();
            services.Configure<JsonWebTokenOptions>(configuration.GetSection(nameof(JsonWebTokenOptions)));

            var jwtOptions = configuration.GetSection(nameof(JsonWebTokenOptions)).Get<JsonWebTokenOptions>();

            services.AddAuthentication(options => {
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

            services.AddAuthorization();

            return services;
        }
    }
}
