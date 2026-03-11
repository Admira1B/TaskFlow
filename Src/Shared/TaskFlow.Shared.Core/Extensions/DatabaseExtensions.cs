using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Shared.Core.Extensions {
    public static class DatabaseExtensions {
        public async static Task<WebApplication> AddDataBaseMigration<TContext>(this WebApplication app, ILogger logger)
            where TContext : DbContext {
            try {
                logger.Info("Starting database migration for {DbContextType}", typeof(TContext).Name);

                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();

                if (!await context.Database.CanConnectAsync()) {
                    logger.Warn("Database does not exist or cannot be connected. Attempting to create...");
                }

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                var pendingMigrationsList = pendingMigrations.ToList();

                if (!pendingMigrationsList.Any()) {
                    logger.Info("No pending migrations for {DbContextType}", typeof(TContext).Name);
                    return app;
                }

                logger.Info(
                    "Applying {Count} pending migration(s) for {DbContextType}: {Migrations}",
                    pendingMigrationsList.Count,
                    typeof(TContext).Name,
                    string.Join(", ", pendingMigrationsList));

                await context.Database.MigrateAsync();

                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

                logger.Info("Successfully applied migrations. Total applied migrations: {Count}", appliedMigrations.Count());

                return app;
            } catch (OperationCanceledException) {
                logger.Warn("Database migration was cancelled");
                throw;
            } catch (Exception ex) {
                logger.Error("Failed to apply database migrations for {DbContextType}", ex, typeof(TContext).Name);
                throw;
            }
        }

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
