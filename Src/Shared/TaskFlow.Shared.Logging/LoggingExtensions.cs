using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Logging {
    public static class LoggingExtensions {
        public static Core.Interfaces.ILogger CreateStartupLogger(string serviceName) {
            LogManager.Setup()
                .LoadConfigurationFromAppSettings();

            var logger = LogManager.GetLogger(serviceName);

            return new Logger(logger);
        }

        public static IServiceCollection AddApplicationLogging(this IServiceCollection services, WebApplicationBuilder builder, string serviceName) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            services.AddHttpContextAccessor();

            services.AddSingleton<Core.Interfaces.ILogger>(provider =>
            {
                var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var nlogLogger = LogManager.GetLogger(serviceName);
                return new Logger(nlogLogger, contextAccessor);
            });

            return services;
        }
    }
}
