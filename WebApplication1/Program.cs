using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using WebApplication1.Middlewares;
using WebApplication1.Services;
using WebApplication1.Workers;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSerilog(config =>
{
    config
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss zzz} {Level:u3}] [{SourceContext} trace:{trace_id}]{NewLine}{Message}{NewLine}{Exception}{NewLine}");
});

services.AddTransient<TraceIdMiddleware>();

services.AddSingleton<ShippingQueue>();
services.AddHostedService<ShippingConsume>();

services.AddControllers();
services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<TraceIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options =>
{
    options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

await app.RunAsync();