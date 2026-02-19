using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Identity.API.Extensions {
    public static class WebApplicationExtensions {
        public async static Task<WebApplication> AddDataBaseMigration<TContext>(this WebApplication app, Shared.Core.Interfaces.ILogger logger) where TContext : DbContext {
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
                    pendingMigrationsList.Count.ToString(),
                    typeof(TContext).Name,
                    string.Join(", ", pendingMigrationsList));

                await context.Database.MigrateAsync();

                var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

                logger.Info("Successfully applied migrations. Total applied migrations: {Count}", appliedMigrations.Count().ToString());

                return app;
            } catch (OperationCanceledException) {
                logger.Warn("Database migration was cancelled");
                throw;
            } catch (Exception ex) {
                logger.Error("Failed to apply database migrations for {DbContextType}", ex, typeof(TContext).Name);
                throw;
            }
        }
    }
}
