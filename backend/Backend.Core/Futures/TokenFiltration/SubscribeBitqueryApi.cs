using System.Text.Json;
using Backend.Core.Interfaces.Bitquery;
using Backend.Core.Interfaces.Bitquery.Types;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.TokenFiltration;


public record SubscribeBitqueryApiCommand() : IRequest;

public class SubscribeBitqueryApi : IRequestHandler<SubscribeBitqueryApiCommand>
{
    private readonly ILogger<SubscribeBitqueryApi> _logger;
    private readonly IMediator _mediator;
    private readonly IBitqueryClient _bitqueryClient;

    public SubscribeBitqueryApi(ILogger<SubscribeBitqueryApi> logger, IMediator mediator, IBitqueryClient bitqueryClient)
    {
        _logger = logger;
        _mediator = mediator;
        _bitqueryClient = bitqueryClient;
    }

    public Task Handle(SubscribeBitqueryApiCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search new tokens");

        try
        {
            _bitqueryClient.SubscribeOnNewTokens(response =>
            {
                _logger.LogInformation("new token: " + JsonSerializer.Serialize(response));
                // _mediator.Send(new FilterTokensCommand(response), cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }
        
        return Task.CompletedTask;
    }
}