using GimenaCreations.Contracts;
using GimenaCreations.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GimenaCreations.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{    
    private readonly ILogger<NotificationsController> _logger;
    private readonly IBus bus;

    public NotificationsController(ILogger<NotificationsController> logger, IBus bus)
    {
        _logger = logger;
        this.bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Webhook([FromBody] Webhook webhook)
    {
        _logger.LogInformation("Webhook: {webhook}", JsonSerializer.Serialize(webhook));
        await bus.Publish(new WebhookNotification(webhook.Data.Id));
        return Ok();
    }
}
