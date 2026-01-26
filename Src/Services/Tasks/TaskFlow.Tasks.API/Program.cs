using NLog;
using NLog.Web;
using TaskFlow.Tasks.API.Composition;

namespace TaskFlow.Tasks.API {
    public class Program {
        public static void Main(string[] args) {
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try {
                logger.Info("Starting Tasks Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                var app = builder.Build();

                app.ConfigurePipeline();

                logger.Info($"Environment: {app.Environment.EnvironmentName}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");
                logger.Info("Tasks Service started successfully. Press Ctrl+C to shut down.");

                app.Run();

                logger.Info("Tasks Service is shutting down...");
            } catch (Exception ex) {
                logger.Fatal("Stopped Tasks Service because of exception", ex);
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
