using Serilog.Context;

namespace KofCApiProxy.Middleware.Logging;

public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        string tenantId = string.Empty;

        var routes = context.Request.RouteValues;
        object? tenantIdRouteParam = null;

        routes?.TryGetValue("tenantId", out tenantIdRouteParam);

        if (tenantIdRouteParam != null)
        {
            tenantId = (string)tenantIdRouteParam;
        }

        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        using (LogContext.PushProperty("TenantId", tenantId))
        using (LogContext.PushProperty("RequestMethod", context.Request.Method))
        {
            return _next(context);
        }
    }
}
