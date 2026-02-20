using NLog;
using TaskFlow.Shared.Core.Helpers;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Core.Extensions;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Tasks.API.Composition;
using TaskFlow.Tasks.Infrastructure.SqlServer;

namespace TaskFlow.Tasks.API {
    public class Program {
        public static async Task Main(string[] args) {
            var serviceName = ServiceHelper.GetServiceName();
            var logger = LoggingExtensions.CreateStartupLogger(serviceName);

            try {
                logger.Info("Starting Tasks Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                if (builder.Environment.IsDevelopment()) {
                    var serviceOpts = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()!;

                    builder.WebHost.ConfigureKestrel(options => {
                        options.ListenAnyIP(serviceOpts.Port);
                    });
                }

                var app = builder.Build();

                await app.AddDataBaseMigration<TasksServiceDbContext>(logger);

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
