using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Shared.Core.Middlewares {
    public class RequestLoggingMiddleware(ILogger logger, RequestDelegate _next) {
        private const long SlowDurationTiming = 3000;

        private readonly ILogger _logger = logger;
        private readonly RequestDelegate _next = _next;
        private readonly HashSet<string> _loggingMethods = ["POST", "PUT", "PATCH", "DELETE"];

        public async Task InvokeAsync(HttpContext context) {
            var timer = Stopwatch.StartNew();

            await _next(context);
            timer.Stop();

            var path = context.Request.Path.ToString().ToLower();
            path = Regex.Replace(path, @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", "{id}");

            var method = context.Request.Method.ToUpper();
            var statusCode = context.Response.StatusCode;
            var duration = timer.ElapsedMilliseconds;

            if (!_loggingMethods.Contains(method)) {
                return;
            }

            var message = $"{method} {path} - {statusCode} ({duration}ms)";

            switch (statusCode) {
                case >= 500:
                    _logger.Error(message);
                    break;
                case >= 400:
                    _logger.Warn(message);
                    break;
                default:
                    if (duration > SlowDurationTiming) {
                        _logger.Warn($"{message} [SLOW]");
                        break;
                    }

                    _logger.Info(message);
                    break;
            }
        }
    }
}
