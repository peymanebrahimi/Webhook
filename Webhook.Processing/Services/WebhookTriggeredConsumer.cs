using MassTransit;
using System.Text.Json;
using Webhook.Contracts;
using Webhook.Processing.Data;
using Webhook.Processing.Models;

namespace Webhook.Processing.Services;

internal sealed class WebhookTriggeredConsumer(
    IHttpClientFactory httpClientFactory,
    WebhookDbContext dbContext) : IConsumer<WebhookTriggered>
{
    public async Task Consume(ConsumeContext<WebhookTriggered> context)
    {
        var httpClient = httpClientFactory.CreateClient();
        var payload = new WebhookPayload
        {
            Id = Guid.NewGuid(),
            EventType = context.Message.EventType,
            SubscriptionId = context.Message.SubscriptionId,
            Timestamp = DateTime.UtcNow,
            Data = context.Message.Data,
        };
        var jsonPayload = JsonSerializer.Serialize(payload);

        try
        {
            var response = await httpClient.PostAsJsonAsync(context.Message.WebhookUrl, payload);
            response.EnsureSuccessStatusCode();

            var attempt = new WebhookDeliveryAttempt
            {
                Id = Guid.NewGuid(),
                WebhookSubscriptionId = context.Message.SubscriptionId,
                Payload = jsonPayload,
                ResponseStatusCode = (int)response.StatusCode,
                CreatedAt = DateTime.UtcNow,
                Success = response.IsSuccessStatusCode,
            };

            dbContext.WebhookDeliveryAttempts.Add(attempt);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {

            var attempt = new WebhookDeliveryAttempt
            {
                Id = Guid.NewGuid(),
                WebhookSubscriptionId = context.Message.SubscriptionId,
                Payload = jsonPayload,
                ResponseStatusCode = null,
                CreatedAt = DateTime.UtcNow,
                Success = false,
            };

            dbContext.WebhookDeliveryAttempts.Add(attempt);
            await dbContext.SaveChangesAsync();
        }
    }
}
