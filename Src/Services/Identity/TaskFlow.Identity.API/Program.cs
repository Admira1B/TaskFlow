using NLog;
using TaskFlow.Shared.Core.Helpers;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Identity.API.Composition;
using TaskFlow.Identity.Infrastructure.SqlServer;

namespace TaskFlow.Identity.API {
    public class Program {
        public static async Task Main(string[] args) {
            var serviceName = ServiceHelper.GetServiceName();
            var logger = LoggingExtensions.CreateStartupLogger(serviceName);

            try {
                logger.Info("Starting Identity Service...");
                var version = ApplicationHelper.GetApplicationVersion();

                var builder = WebApplication.CreateBuilder(args);
                builder.ConfigureServices();

                if (builder.Environment.IsDevelopment()) {
                    var serviceOpts = builder.Configuration.GetServiceOptions();

                    builder.WebHost.ConfigureKestrel(options => {
                        options.ListenAnyIP(serviceOpts.Port);
                    });
                }

                var app = builder.Build();
                await app.AddDataBaseMigration<IdentityServiceDbContext>(logger);
                app.ConfigurePipeline();

                logger.Info($"Environment: {app.Environment.EnvironmentName} | Version: {version}");
                logger.Info($"Application Name: {builder.Environment.ApplicationName}");
                logger.Info("Identity Service started successfully");

                app.Run();

                logger.Info("Identity Service is shutting down...");
            } catch (OperationCanceledException) { 
                logger.Info("Identity Service startup was canceled");
            } catch (Exception ex) {
                logger.Fatal("Stopped Identity Service because of exception", ex);
                throw;
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}
