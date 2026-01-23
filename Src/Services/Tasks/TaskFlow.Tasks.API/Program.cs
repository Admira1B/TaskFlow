using TaskFlow.Tasks.API.Composition;

namespace TaskFlow.Tasks.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<Shared.Core.Interfaces.ILogger>();

            try {
                logger.Info("Starting Tasks Service...");
                logger.Info($"Environment: {app.Environment.EnvironmentName}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");

                app.ConfigurePipeline();

                logger.Info("Tasks Service started successfully");

                app.Run();

                logger.Info("Tasks Service is shutting down...");
            } catch (Exception ex) {
                logger.Error("Tasks Service failed to start", ex);
                throw;
            }
        }
    }
}
