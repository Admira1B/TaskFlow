using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Application.Mapping;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Domain.Contracts.Repositories;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Infrastructure.SqlServer;
using TaskFlow.Identity.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Identity.API {
    internal static class ServiceCollectionExtensions {
        public static IServiceCollection AddIdentityServiceDependencies(this IServiceCollection services) {
            // DbContext
            services.AddDbContext<IdentityDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

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
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            // Data Access
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Services
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<AuthService>();

            // AutoMapper
            services.AddSingleton<IMapper>(x => {
                var configExpression = new MapperConfigurationExpression();
                configExpression.AddProfile<IdentityMapperProfile>();

                var config = new MapperConfiguration(
                    configExpression, null
                );

                return config.CreateMapper();
            });

            return services;
        }
    }
}
