using NLog;
using NLog.Web;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Identity.API.Composition;
using TaskFlow.Identity.API.Extensions;
using TaskFlow.Identity.Infrastructure.SqlServer;

namespace TaskFlow.Identity.API {
    public class Program {
        public static async Task Main(string[] args) {
            var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            try {
                logger.Info("Starting Identity Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                if (builder.Environment.IsDevelopment()) {
                    var serviceOpts = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()!;

                    builder.WebHost.ConfigureKestrel(options => {
                        options.ListenAnyIP(serviceOpts.PortParsed);
                    });
                }

                var app = builder.Build();

                await app.AddDataBaseMigration<IdentityServiceDbContext>(logger);

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
