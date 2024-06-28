using System.Text.Json;
using Backend.Core.Gateways;
using Backend.Core.Interfaces.Bitquery;
using Backend.Core.Interfaces.TokenFilter;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.TokenFiltration;


public record SubscribeBitqueryApiCommand() : IRequest;

public class SubscribeBitqueryApi : IRequestHandler<SubscribeBitqueryApiCommand>
{
    private readonly ILogger<SubscribeBitqueryApi> _logger;
    private readonly IMediator _mediator;
    private readonly IBitqueryClient _bitqueryClient;
    private readonly ITokenFilter _tokenFilter;
    private readonly ITokenGateway _tokenGateway;
    private readonly IFullTokenInformationSearcher _informationSearcher;

    public SubscribeBitqueryApi(ILogger<SubscribeBitqueryApi> logger, IMediator mediator, IBitqueryClient bitqueryClient, ITokenFilter tokenFilter, ITokenGateway tokenGateway, IFullTokenInformationSearcher informationSearcher)
    {
        _logger = logger;
        _mediator = mediator;
        _bitqueryClient = bitqueryClient;
        _tokenFilter = tokenFilter;
        _tokenGateway = tokenGateway;
        _informationSearcher = informationSearcher;
    }

    public Task Handle(SubscribeBitqueryApiCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search new tokens");

        try
        {
            _bitqueryClient.SubscribeOnNewTokens(async response =>
            {
                _logger.LogInformation("new token: " + JsonSerializer.Serialize(response));

                // TODO: filtration soon
                // var fullTokenInformation = _informationSearcher.Search(response);
                // var isValid = await _tokenFilter.Filter(fullTokenInformation);
                // if (isValid == false)
                //     return;

                // TODO: map FullTokenInformation to CryptoToken
                await _tokenGateway.AddAsync(new CryptoToken());
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }
        
        return Task.CompletedTask;
    }
}