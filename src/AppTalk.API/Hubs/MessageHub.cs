using AppTalk.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AppTalk.API.Hubs;

[Authorize]
public class MessageHub(ILogger<MessageHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        logger.LogDebug("User {UserId} connected to MessageHub. Connection ID: {ConnectionId}", Context.User.GetUserId(), Context.ConnectionId);
        await base.OnConnectedAsync();
    }
}