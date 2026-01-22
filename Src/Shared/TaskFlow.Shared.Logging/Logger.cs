using System.Security.Claims;
using NLog;
using Microsoft.AspNetCore.Http;

namespace TaskFlow.Shared.Logging {
    public class Logger(ILogger logger, IHttpContextAccessor? contextAccessor = null) {
        private readonly ILogger _logger = logger;
        private readonly IHttpContextAccessor? _contextAccessor = contextAccessor;

        public void Debug(string message, params string[] args) {
            Log(LogLevel.Debug, message, args: args);
        }

        public void Info(string message, params string[] args) { 
            Log(LogLevel.Info, message, args: args);
        }

        public void Warn(string message, params string[] args) {
            Log(LogLevel.Warn, message, args: args);
        }

        public void Error(string message, Exception? exception = null, params string[] args) { 
            Log(LogLevel.Error, message, exception, args: args);
        }

        public void Fatal(string message, Exception? exception = null, params string[] args) { 
            Log(LogLevel.Error, message, exception, args: args);
        }

        private void Log(LogLevel loggingLevel, string message, Exception? exception = null, params string[] args) {
            var logEvent = new LogEventInfo(loggingLevel, _logger.Name, null, message, args) {
                Exception = exception
            };

            if (_contextAccessor is not null) {
                AddHttpContext(logEvent);
            }

            _logger.Log(logEvent);
        }

        private void AddHttpContext(LogEventInfo logEvent) {
            var context = _contextAccessor?.HttpContext;
            if (context is null) {
                return;
            }

            logEvent.Properties["TraceId"] = context.TraceIdentifier;
            logEvent.Properties["RequestPath"] = context.Request.Path;
            logEvent.Properties["RequestMethod"] = context.Request.Method;

            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
                logEvent.Properties["UserId"] = userId;
        }
    }
}
