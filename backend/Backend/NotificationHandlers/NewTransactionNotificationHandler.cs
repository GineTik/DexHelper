using Backend.Constants;
using Backend.Core.Futures.Token.Trackers.TrackNewTransactions;
using Backend.SignalRHubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.NotificationHandlers;

public class NewTransactionNotificationHandler : INotificationHandler<NewTransactionNotification>
{
    private readonly IHubContext<TokensHub, ITypedTokensHub> _hubContext;
    
    public NewTransactionNotificationHandler(IHubContext<TokensHub, ITypedTokensHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(NewTransactionNotification notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients
            .Group(SignalRGroupsConstants.TrackTokenTransaction(notification.CryptoTokenAddress))
            .ReceiveNewTransaction(notification);
    }
}