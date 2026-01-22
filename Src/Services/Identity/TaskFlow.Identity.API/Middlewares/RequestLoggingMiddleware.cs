using System.Diagnostics;
using TaskFlow.Shared.Logging;

namespace TaskFlow.Identity.API.Middlewares {
    public class RequestLoggingMiddleware(Logger logger, RequestDelegate _next) {
        private readonly Logger _logger = logger;
        private readonly RequestDelegate _next = _next;

        public async Task InvokeAsync(HttpContext context) {
            var stopwatch = Stopwatch.StartNew();

            try {
                await _next(context);
            } finally {
                stopwatch.Stop();
                await LogRequestAsync(context, stopwatch.ElapsedMilliseconds);
            }
        }

        private async Task LogRequestAsync(HttpContext context, long elapsedMilliseconds) {
            
        }
    }
}
