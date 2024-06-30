using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Backend.Core.Futures.TokenFiltration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class TokensController : Controller
{
    private readonly IMediator _mediator;
    public TokensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/ws/new-tokens")]
    public async Task SubscribeOnNewTokens()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var websocketClient = await HttpContext.WebSockets.AcceptWebSocketAsync();
            while (websocketClient.State != WebSocketState.Closed && websocketClient.State != WebSocketState.Aborted)
            {
                var newTokens = await _mediator.Send(new GetNewTokensRequest { Page = 1 });

                if (websocketClient.State == WebSocketState.Open)
                {
                    var message = JsonSerializer.Serialize(newTokens);
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    await websocketClient.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                
                await Task.Delay(3000);
            }
        }
        else
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}