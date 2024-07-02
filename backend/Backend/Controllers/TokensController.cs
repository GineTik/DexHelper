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
}