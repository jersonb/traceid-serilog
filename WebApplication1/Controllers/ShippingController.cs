using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("")]
public class ShippingController(
    ILogger<ShippingController> logger,
    ShippingQueue shippingQueue)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromHeader(Name = "X-trace-id")] string? traceId, 
        ShippingCreate create)
    {
        logger.LogInformation("Received shipping create request: {@Request}", create);

        foreach (var _ in Enumerable.Range(0, create.Count))
        {
            shippingQueue.Enqueue(new Shipping(create.Id, traceId));
        }

        return Created();
    }
}

public record ShippingCreate(int Count, string Id);