using GimenaCreations.Contracts;
using GimenaCreations.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace GimenaCreations.Consumers;

[Authorize]
public sealed class OrderStatusChangedToSubmittedConsumer : IConsumer<OrderStatusChangedToSubmitted>
{
    private readonly ILogger<OrderStatusChangedToSubmittedConsumer> _logger;
    private readonly IHubContext<NotificationHub> hubContext;

    public OrderStatusChangedToSubmittedConsumer(ILogger<OrderStatusChangedToSubmittedConsumer> logger, IHubContext<NotificationHub> hubContext)
    {
        _logger = logger;
        this.hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedToSubmitted> context)
    {
        _logger.LogInformation("Received message: {Message}", JsonSerializer.Serialize(context.Message));
        await hubContext.Clients.User(context.Message.UserId).SendAsync("UpdatedOrderState", new { context.Message.OrderId, status = context.Message.OrderStatus.GetDisplayName() });
    }
}
