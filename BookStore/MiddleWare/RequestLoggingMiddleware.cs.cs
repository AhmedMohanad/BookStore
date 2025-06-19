namespace BookStore.MiddleWare
{
    public class RequestLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log request info
            _logger.LogInformation(" HTTP {Method} {Path}", context.Request.Method, context.Request.Path);

            // Call the next middleware in the pipeline
            await _next(context);

            // Log response status
            _logger.LogInformation(" HTTP {StatusCode}", context.Response.StatusCode);
        }
    }
}
