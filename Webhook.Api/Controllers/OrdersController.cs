using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webhook.Api.Data;
using Webhook.Api.Models;
using Webhook.Api.Services;

namespace Webhook.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(
    WebhookDbContext dbContext,
    WebhookDispatcher webhookDispatcher) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var orders = await dbContext.Orders.ToListAsync();

        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateOrderRequest request)
    {
        var order = new Order(Guid.NewGuid(), request.CustomerName, request.Amount, DateTime.UtcNow);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        
        await webhookDispatcher.DispatchAsync("order.created", order);
        return Ok(order);
    }
}