using Microsoft.OpenApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Application.Mapping;
using TaskFlow.Identity.Domain.Contracts.Repositories;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Identity.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Identity.API {
    internal static class ServiceCollectionExtensions {
        public static IServiceCollection AddIdentityServiceDependencies(this IServiceCollection services) {
            // Adding controllers
            services.AddControllers();

            // Adding documentation
            services.AddOpenApi();
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "TaskFlow Identity Service",
                    Contact = new OpenApiContact {
                        Name = "Vlad Reizenbuk", Email = "vreizenbuk@mail.ru"
                    }
                })
            );

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

            // ASP Identity
            services.AddIdentity<User, Role>(options => {
                // Password Options
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                
                // User Options
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityServiceDbContext>()
            .AddDefaultTokenProviders();

            // MediatoR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(IdentityMapperProfile).Assembly);

            return services;
        }
    }
}
