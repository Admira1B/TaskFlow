using TaskFlow.Identity.API.Composition;

namespace TaskFlow.Identity.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<Shared.Core.Interfaces.ILogger>();

            try {
                logger.Info("Starting Identity Service...");
                logger.Info($"Environment: {app.Environment.EnvironmentName}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");

                app.ConfigurePipeline();

                logger.Info("Identity Service started successfully");

                app.Run();

                logger.Info("Identity Service is shutting down...");
            } catch (Exception ex) {
                logger.Error("Identity Service failed to start", ex);
                throw;
            }
        }
    }
}
