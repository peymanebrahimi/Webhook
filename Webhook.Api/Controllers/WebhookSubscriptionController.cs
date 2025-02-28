using Microsoft.AspNetCore.Mvc;
using Webhook.Api.Data;
using Webhook.Api.Models;

namespace Webhook.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WebhookSubscriptionController(WebhookDbContext dbContext)
    : ControllerBase
{
    // [HttpGet]
    // public async Task<IActionResult> Get(string eventType)
    // {
    //     var webhookSubscriptions = webhookSubscriptionRepository.GetByEventType(eventType);
    //
    //     return Ok(webhookSubscriptions);
    // }

    [HttpPost]
    public async Task<IActionResult> Post(CreateWebhookRequest request)
    {
        var webhookSubscription =
            new WebhookSubscription(Guid.NewGuid(), request.EventType, request.WebhookUrl, DateTime.UtcNow);

        dbContext.WebhookSubscriptions.Add(webhookSubscription);
        await dbContext.SaveChangesAsync();
        
        return Ok(webhookSubscription);
    }
}