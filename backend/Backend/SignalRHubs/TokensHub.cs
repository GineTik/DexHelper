using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Backend.Core.Futures.TokenFiltration;
using Backend.Core.Futures.TokenFiltration.Types;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.SignalRHubs;

public interface ITypedTokensHub
{
    Task ReceiveNewToken(TokenResponse token);
}

public class TokensHub : Hub<ITypedTokensHub>
{
    private readonly IMediator _mediator;
    
    public TokensHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async IAsyncEnumerable<GetNewTokensResponse> ReceiveNewTokens([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            var page = await _mediator.Send(new GetNewTokensRequest { Page = 1 }, cancellationToken);
            yield return page;
            await Task.Delay(2000, cancellationToken);
        }
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine("connected " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("disconected " + Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}