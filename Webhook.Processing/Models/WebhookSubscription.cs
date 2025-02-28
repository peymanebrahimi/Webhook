﻿namespace Webhook.Processing.Models;

public sealed record WebhookSubscription(Guid Id, string EventType, string WebhookUrl, DateTime CreatedOnUtc);

public sealed record CreateWebhookRequest(string EventType, string WebhookUrl);