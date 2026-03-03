using NLog;
using TaskFlow.Shared.Core.Helpers;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Gateway.Composition;

namespace TaskFlow.Gateway {
    public class Program {
        public async static Task Main(string[] args) {
            var serviceName = ServiceHelper.GetServiceName();
            var logger = LoggingExtensions.CreateStartupLogger(serviceName);

            try {
                logger.Info("Starting Gateway Service...");
                var version = ApplicationHelper.GetApplicationVersion();

                var builder = WebApplication.CreateBuilder(args);
                builder.ConfigureServices();

                var app = builder.Build();
                await app.ConfigurePipelineAsync();

                logger.Info($"Environment: {app.Environment.EnvironmentName} | Version: {version}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");
                logger.Info("Gateway Service started successfully");

                app.Run();

                logger.Info("Gateway Service is shutting down...");
            } catch (OperationCanceledException) {
                logger.Info("Gateway Service startup was canceled");
            } catch (Exception ex) {
                logger.Fatal("Stopped Gateway Service because of exception", ex);
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}