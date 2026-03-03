using WebApplication1.Services;

namespace WebApplication1.Workers;

public class ShippingConsume(
    ILogger<ShippingConsume> logger,
    ShippingQueue shippingQueue)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (shippingQueue.TryDequeue(out var shipping) && shipping is { })
            {
                logger.LogInformation("Shipping {Id} is being processed", shipping.Id);
                await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 6)), stoppingToken);

                if (DateTime.Now.Second % 2 == 0)
                {
                    logger.LogError("Problem on process Shipping {Id}", shipping.Id);
                }
            }
        }
    }
}