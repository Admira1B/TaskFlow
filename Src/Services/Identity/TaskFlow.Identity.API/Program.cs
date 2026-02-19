using NLog;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Logging.Extensions;
using TaskFlow.Identity.API.Extensions;
using TaskFlow.Identity.API.Composition;
using TaskFlow.Identity.Infrastructure.SqlServer;

namespace TaskFlow.Identity.API {
    public class Program {
        public static async Task Main(string[] args) {
            var serviceName = GetServiceName();
            var logger = LoggingExtensions.CreateStartupLogger(serviceName);

            try {
                logger.Info("Starting Identity Service...");
                var builder = WebApplication.CreateBuilder(args);

                builder.ConfigureServices();

                if (builder.Environment.IsDevelopment()) {
                    var serviceOpts = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()!;

                    builder.WebHost.ConfigureKestrel(options => {
                        options.ListenAnyIP(serviceOpts.Port);
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

        public static string GetServiceName() {
            var unknownName = "not-configured";

            try {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                return config["ServiceOptions:Name"] ?? unknownName;
            } catch {
                return unknownName;
            }
        }
    }
}
