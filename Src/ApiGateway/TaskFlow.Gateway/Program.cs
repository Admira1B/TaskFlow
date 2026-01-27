using NLog;
using NLog.Web;
using TaskFlow.Gateway.Composition;

namespace TaskFlow.Gateway {
    public class Program {
        public async static Task Main(string[] args) {
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try {
                logger.Info("Starting Gateway Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                var app = builder.Build();

                await app.ConfigurePipelineAsync();

                logger.Info($"Environment: {app.Environment.EnvironmentName}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");
                logger.Info("Gateway Service started successfully. Press Ctrl+C to shut down.");

                app.Run();

                logger.Info("Gateway Service is shutting down...");
            } catch (Exception ex) {
                logger.Fatal("Stopped Gateway Service because of exception", ex);
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}