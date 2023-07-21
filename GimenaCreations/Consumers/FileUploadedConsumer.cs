using GimenaCreations.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace GimenaCreations.Consumers;

[Authorize]
public class FileUploadedConsumer : IConsumer<FileUploaded>
{    
    private readonly ILogger<OrderStatusChangedToSubmittedConsumer> _logger;
    private readonly IHubContext<NotificationHub> hubContext;

    public FileUploadedConsumer(ILogger<OrderStatusChangedToSubmittedConsumer> logger, IHubContext<NotificationHub> hubContext)
    {
        _logger = logger;
        this.hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<FileUploaded> context)
    {
        _logger.LogInformation("Received message: {Message}", JsonSerializer.Serialize(context.Message));
        await hubContext.Clients.User(context.Message.UserId).SendAsync("FileUploaded", new { context.Message.OrderId });
    }
}
