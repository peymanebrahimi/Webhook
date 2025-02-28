using Microsoft.EntityFrameworkCore;
using Webhook.Api.Models;

namespace Webhook.Api.Data;

public class WebhookDbContext(DbContextOptions<WebhookDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }
    public DbSet<WebhookDeliveryAttempt> WebhookDeliveryAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            b.ToTable("orders");
            b.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<WebhookSubscription>(b =>
        {
            b.ToTable("subscriptions", "webhooks");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<WebhookDeliveryAttempt>(b =>
        {
            b.ToTable("delivery_attempts", "webhooks");
            b.HasKey(x => x.Id);

            b.HasOne<WebhookSubscription>()
                .WithMany()
                .HasForeignKey(x => x.WebhookSubscriptionId);

        });
    }
}