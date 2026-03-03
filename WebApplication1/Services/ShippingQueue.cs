using WebApplication1.Extensions;

namespace WebApplication1.Services;

public class ShippingQueue
{
    private readonly Queue<Shipping> _queue = [];

    public void Enqueue(Shipping shipping)
    {
        shipping = SetShippingTraceId(shipping);

        _queue.Enqueue(shipping);
    }

    public bool TryDequeue(out Shipping? shipping)
    {
        if (_queue.TryDequeue(out shipping))
        {
            shipping = SetShippingTraceId(shipping);
            return true;
        }
        return false;
    }

    private static Shipping SetShippingTraceId(Shipping shipping)
    {
        return shipping with { RequestId = shipping.RequestId.ToTraceId() };
    }
}

public record Shipping(string Id, string? RequestId);