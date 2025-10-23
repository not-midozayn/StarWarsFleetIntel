using System.Diagnostics;

namespace StarWarsFleetIntel.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
                              ?? Guid.NewGuid().ToString();

            context.Items["CorrelationId"] = correlationId;
            context.Response.Headers.TryAdd(CorrelationIdHeader, correlationId);

            var activity = Activity.Current;
            if (activity != null)
            {
                activity.SetTag("correlation.id", correlationId);
            }

            await _next(context);
        }
    }
}
