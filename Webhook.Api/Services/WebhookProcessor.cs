//using System.Diagnostics;
//using System.Threading.Channels;
//using Webhook.Api.OpenTelemetry;

//namespace Webhook.Api.Services;

//internal sealed class WebhookProcessor(
//    IServiceScopeFactory serviceScopeFactory,
//    Channel<WebhookDispatched> webhookChannel) : BackgroundService
//{
//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        await foreach (var item in webhookChannel.Reader.ReadAllAsync(stoppingToken))
//        {
//            using var activity = DiagnosticConfig.ActivitySource.StartActivity($"{item.EventType} process webhook",
//                ActivityKind.Internal);

//            using var scope = serviceScopeFactory.CreateScope();
//            var dispatcher = scope.ServiceProvider.GetRequiredService<WebhookDispatcher>();
//            await dispatcher.ProcessAsync(item.EventType, item.Data);
//        }
//    }
//}
