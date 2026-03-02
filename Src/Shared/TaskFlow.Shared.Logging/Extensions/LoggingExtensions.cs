using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Core.Options;
using Microsoft.Extensions.Configuration;

namespace TaskFlow.Shared.Logging.Extensions {
    public static class LoggingExtensions {
        public static Core.Interfaces.ILogger CreateStartupLogger(string serviceName) {
            LogManager.Setup()
                .LoadConfigurationFromAppSettings();

            var logger = LogManager.GetLogger(serviceName);

            return new Logger(logger);
        }

        public static IServiceCollection AddLogging(this IServiceCollection services, WebApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            services.AddHttpContextAccessor();

            var serviceOptions = builder.Configuration.GetServiceOptions();

            services.AddSingleton<Core.Interfaces.ILogger>(provider =>
            {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var nlogLogger = LogManager.GetLogger(serviceOptions.Name);
                return new Logger(nlogLogger, contextAccessor);
            });

            return services;
        }
    }
}
