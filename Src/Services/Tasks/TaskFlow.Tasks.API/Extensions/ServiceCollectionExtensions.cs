using System.Text;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskFlow.Tasks.Application.Mapping;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Domain.Options;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Tasks.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Tasks.API.Extensions {
    internal static class ServiceCollectionExtensions {
        public static IServiceCollection AddTaskServiceDependencies(this IServiceCollection services, IConfiguration configuration) {
            // Adding controllers
            services.AddControllers();

            // Adding documentation
            services.AddSwaggerGen(options => {
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
            services.AddDbContext<TaskServiceDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(TaskServiceDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });


            // Data Access
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            services.AddScoped<ITaskGroupRepository, TaskGroupRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();

            // MediatoR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateCommentCommandHandler).Assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(TaskServiceMapperProfile).Assembly);

            // JsonWebToken Authentication & Authorization
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
