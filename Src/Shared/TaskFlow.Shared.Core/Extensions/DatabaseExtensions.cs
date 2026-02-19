using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Extensions {
    public static class DatabaseExtensions {
        public static IServiceCollection AddDbContextWithMigrations<TContext>(this IServiceCollection services, WebApplicationBuilder builder)
            where TContext : DbContext {
            var migrationsAssembly = typeof(TContext).Assembly.FullName;
            var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString")
                ?? throw new InvalidOperationException("Connection string 'SqlServerConnectionString' not found.");

            services.AddDbContext<TContext>(options =>  
            {
                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            return services;
        }
    }
}
