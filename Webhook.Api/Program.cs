using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Threading.Channels;
using Webhook.Api;
using Webhook.Api.Data;
using Webhook.Api.OpenTelemetry;
using Webhook.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();
builder.Services.AddScoped<WebhookDispatcher>();

//builder.Services.AddHostedService<WebhookProcessor>();
//builder.Services.AddSingleton(_ =>
//{
//    return Channel.CreateBounded<WebhookDispatch>(new BoundedChannelOptions(100)
//    {
//        FullMode = BoundedChannelFullMode.Wait
//    });
//});

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.SetKebabCaseEndpointNameFormatter();

    busConfig.AddConsumer<WebhookDispatchedConsumer>();
    busConfig.AddConsumer<WebhookTriggeredConsumer>();

    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });
});
//builder.Services.AddHttpClient<WebhookDispatcher>();
builder.Services.AddDbContext<WebhookDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("webhooks"));
});

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
        .AddSource(DiagnosticConfig.ActivitySource.Name)
        .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
        .AddNpgsql();
    });

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();