using NLog;
using NLog.Web;
using TaskFlow.Identity.API.Composition;

namespace TaskFlow.Identity.API {
    public class Program {
        public static void Main(string[] args) {
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try {
                logger.Info("Starting Identity Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                var app = builder.Build();

                app.ConfigurePipeline();

                logger.Info($"Environment: {app.Environment.EnvironmentName}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");
                logger.Info("Identity Service started successfully. Press Ctrl+C to shut down.");

                app.Run();

                logger.Info("Identity Service is shutting down...");
            } catch (Exception ex) {
                logger.Fatal("Stopped Identity Service because of exception", ex);
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
