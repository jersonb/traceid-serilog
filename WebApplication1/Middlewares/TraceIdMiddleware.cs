using WebApplication1.Extensions;

namespace WebApplication1.Middlewares;

public class TraceIdMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var traceIdFromHeader = context.Request.Headers.FirstOrDefault(x => x.Key == "x-trace-id");
        var traceId = traceIdFromHeader.Value.ToString().ToTraceId();

        context.Request.Headers.Remove("x-trace-id");
        context.Request.Headers.Append("x-trace-id", traceId);
        return next(context);
    }
}