using MassTransit;
using Microsoft.EntityFrameworkCore;
using Webhook.Contracts;
using Webhook.Processing.Data;

namespace Webhook.Processing.Services;

internal sealed class WebhookDispatchedConsumer(WebhookDbContext dbContext) : IConsumer<WebhookDispatched>
{
    public async Task Consume(ConsumeContext<WebhookDispatched> context)
    {
        var message = context.Message;

        var subscriptions = await dbContext.WebhookSubscriptions
            .AsNoTracking()
            .Where(x => x.EventType == message.EventType)
            .ToListAsync();

        foreach (var subscription in subscriptions)
        {
            await context.Publish(new WebhookTriggered(
                subscription.Id, message.EventType, subscription.WebhookUrl, message.Data));
        }

        //await context.PublishBatch(subscriptions.Select(subscription =>
        //                new WebhookTriggered(
        //                    subscription.Id,
        //                    message.EventType, 
        //                    subscription.WebhookUrl, 
        //                    message.Data)));
    }
}
