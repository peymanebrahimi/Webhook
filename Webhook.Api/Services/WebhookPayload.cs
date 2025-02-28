namespace Webhook.Api.Services;

public class WebhookPayload
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public DateTime Timestamp { get; set; }
    public object Data { get; set; }
    public string EventType { get; internal set; }
}