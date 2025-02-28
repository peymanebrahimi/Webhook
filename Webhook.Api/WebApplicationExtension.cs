using Microsoft.EntityFrameworkCore;
using Webhook.Api.Data;

namespace Webhook.Api;

public static class WebApplicationExtension
{
    public static async Task ApplyMigrationAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WebhookDbContext>();
        await db.Database.MigrateAsync();
    }
}
