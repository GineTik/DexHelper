using System.Runtime.CompilerServices;
using Backend.Core.Futures.TokenFiltration;
using Backend.Core.Futures.TokenFiltration.Types;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.SignalRHubs;

public class TokensHub : Hub
{
    private readonly IMediator _mediator;
    
    public TokensHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async IAsyncEnumerable<TokenPaginationPage<GetNewTokensResponse>> SubscribeNewToken(int page, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false)
        {
            yield return await _mediator.Send(new GetNewTokensRequest { Page = page }, cancellationToken);
            await Task.Delay(3000, cancellationToken);
        }
    }
}