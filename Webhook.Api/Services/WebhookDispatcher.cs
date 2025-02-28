using MassTransit;
using System.Diagnostics;
using Webhook.Api.OpenTelemetry;
using Webhook.Contracts;

namespace Webhook.Api.Services;

public class WebhookDispatcher(
    //Channel<WebhookDispatch> webhookChannel,
    IPublishEndpoint publishEndpoint
    //,
    //IHttpClientFactory httpClientFactory,
    //WebhookDbContext dbContext
    )
{
    public async Task DispatchAsync<T>(string eventType, T data)
        where T : notnull
    {
        using Activity? activity = DiagnosticConfig.ActivitySource.StartActivity($"{eventType} dispatch webhook");
        activity?.AddTag("event.type", eventType);

        //await webhookChannel.Writer.WriteAsync(new WebhookDispatch(eventType, data, activity?.Id));
        await publishEndpoint.Publish(new WebhookDispatched(eventType, data));
    }
    //public async Task ProcessAsync<T>(string eventType, T data)
    //{
    //    var subscriptions = await dbContext.WebhookSubscriptions
    //        .AsNoTracking()
    //        .Where(x => x.EventType == eventType)
    //        .ToListAsync();

    //    foreach (var webhookSubscription in subscriptions)
    //    {
    //        var httpClient = httpClientFactory.CreateClient();
    //        var payload = new WebhookPayload<T>
    //        {
    //            Id = Guid.NewGuid(),
    //            EventType = webhookSubscription.EventType,
    //            SubscriptionId = webhookSubscription.Id,
    //            Timestamp = DateTime.UtcNow,
    //            Data = data,
    //        };
    //        var jsonPayload = JsonSerializer.Serialize(payload);

    //        try
    //        {
    //            var response = await httpClient.PostAsJsonAsync(webhookSubscription.WebhookUrl, payload);

    //            var attempt = new WebhookDeliveryAttempt
    //            {
    //                Id = Guid.NewGuid(),
    //                WebhookSubscriptionId = webhookSubscription.Id,
    //                Payload = jsonPayload,
    //                ResponseStatusCode = (int)response.StatusCode,
    //                CreatedAt = DateTime.UtcNow,
    //                Success = response.IsSuccessStatusCode,
    //            };

    //            dbContext.WebhookDeliveryAttempts.Add(attempt);
    //            await dbContext.SaveChangesAsync();
    //        }
    //        catch (Exception)
    //        {

    //            var attempt = new WebhookDeliveryAttempt
    //            {
    //                Id = Guid.NewGuid(),
    //                WebhookSubscriptionId = webhookSubscription.Id,
    //                Payload = jsonPayload,
    //                ResponseStatusCode = null,
    //                CreatedAt = DateTime.UtcNow,
    //                Success = false,
    //            };

    //            dbContext.WebhookDeliveryAttempts.Add(attempt);
    //            await dbContext.SaveChangesAsync();
    //        }


    //    }
    //}
}
