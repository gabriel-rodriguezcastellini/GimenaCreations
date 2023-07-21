using Microsoft.AspNetCore.SignalR;

namespace GimenaCreations;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier!);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
