using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class TokensController : Controller
{
    [HttpGet("/ws/new-tokens")]
    public void SubscribeOnNewTokens()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            
        }
        else
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}