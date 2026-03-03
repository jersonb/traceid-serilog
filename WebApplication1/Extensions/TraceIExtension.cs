using System.Diagnostics;
using System.Security.Cryptography;
using Serilog.Context;
using Serilog.Events;

namespace WebApplication1.Extensions;

public static class TraceIExtension
{
    public static string ToTraceId(this string? requestId)
    {
        if (requestId is not { Length: 32 })
        {
            requestId = Activity.Current is { } activity
                ? activity.TraceId.ToHexString()
                : RandomNumberGenerator.GetHexString(32);
        }
        var traceId = Convert.ToUInt64(requestId[16..], 16);
        LogContext.PushProperty("trace_id", new ScalarValue(traceId));
        return requestId;
    }

    public static void SetupTraceId() 
    {
        var source = new ActivitySource(Environment.MachineName, "1.0.0");

        ActivitySource.AddActivityListener(new ActivityListener
        {
            ShouldListenTo = s => s.Name == source.Name,
            Sample = (ref options) => ActivitySamplingResult.AllDataAndRecorded,
            SampleUsingParentId = (ref options) => ActivitySamplingResult.AllDataAndRecorded
        });
    }
}