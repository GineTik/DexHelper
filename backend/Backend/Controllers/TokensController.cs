using Backend.Core.Futures.Token;
using Backend.SignalRHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Controllers;

[Route("tokens")]
public class TokensController : Controller
{
    private readonly IMediator _mediator;
    private readonly IHubContext<TokensHub, ITypedTokensHub> _hubContext;
    
    public TokensController(IMediator mediator, IHubContext<TokensHub, ITypedTokensHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }

    [HttpGet("last-tokens")]
    public async Task<GetNewTokensResponse> GetLastTokens(int page)
    {
        return await _mediator.Send(new GetNewTokensRequest(page));
    }

    [HttpGet("get-info")]
    public async Task<GetTokenInformationResponse> GetInfo(string address)
    {
        return await _mediator.Send(new GetTokenInformationRequest(address));
    }
}