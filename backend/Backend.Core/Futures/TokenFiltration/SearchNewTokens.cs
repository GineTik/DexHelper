using System.Text.Json;
using Backend.Core.Gateways;
using Backend.Core.Interfaces.TokensApi;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.TokenFiltration;


public record SearchNewTokensRequest() : IRequest;

public class SearchNewTokensHandler : IRequestHandler<SearchNewTokensRequest>
{
    private readonly ILogger<SearchNewTokensHandler> _logger;
    private readonly ITokensApiClient _tokensApiClient;
    private readonly ITokenGateway _tokenGateway;

    public SearchNewTokensHandler(ILogger<SearchNewTokensHandler> logger, ITokensApiClient tokensApiClient, ITokenGateway tokenGateway)
    {
        _logger = logger;
        _tokensApiClient = tokensApiClient;
        _tokenGateway = tokenGateway;
    }

    public Task Handle(SearchNewTokensRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search new tokens");

        try
        {
            _tokensApiClient.SubscribeOnNewTokens(async response =>
            {
                _logger.LogInformation("new token: " + JsonSerializer.Serialize(response));

                // TODO: filtration soon
                // var isValid = await _tokenFilter.Filter(fullTokenInformation);
                // if (isValid == false)
                //     return;

                await _tokenGateway.AddAsync(new CryptoToken
                {
                    Name = response.Name,
                    Symbol = response.Symbol,
                    Description = response.Description,
                    ImageUrl = response.ImageUrl,
                    Signature = response.TransactionSignature,
                    CreatedAtUtc = response.CreatedAtUtc,
                    TokenAddress = response.TokenAddress,
                    PoolAddress = response.PoolAddress,
                    UpdatedAtUtc = response.CreatedAtUtc,
                    Website = response.Website,
                    Twitter = response.Twitter,
                    Telegram = response.Telegram,
                });
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }
        
        return Task.CompletedTask;
    }
}