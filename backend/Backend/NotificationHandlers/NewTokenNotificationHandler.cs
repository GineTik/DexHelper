using Backend.Core.Futures.TokenFiltration;
using Backend.SignalRHubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.NotificationHandlers;

public class NewTokenNotificationHandler : INotificationHandler<NewTokenNotification>
{
    private readonly IHubContext<TokensHub, ITypedTokensHub> _hubContext;
    
    public NewTokenNotificationHandler(IHubContext<TokensHub, ITypedTokensHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(NewTokenNotification notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.ReceiveNewToken(notification);
    }
}