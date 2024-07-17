using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Backend.Core.Futures.TokenFiltration;
using Backend.Core.Futures.TokenFiltration.Types;
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
        return await _mediator.Send(new GetNewTokensRequest
        {
            Page = page
        });
    }

    [HttpPost("test")]
    public async Task Test()
    {
        await _hubContext.Clients.All.ReceiveNewToken(new TokenResponse
        {
            Name = "Test #2",
            Symbol = "TEST #2",
            CreatedAtUtc = DateTime.UtcNow
        });
    }
}