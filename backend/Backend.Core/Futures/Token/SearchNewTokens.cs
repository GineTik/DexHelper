using System.Text.Json;
using Backend.Core.Futures.Token.Types;
using Backend.Core.Gateways;
using Backend.Core.Interfaces.Token.TokensApi;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.Token;


public record SearchNewTokensRequest() : IRequest;

public class NewTokenNotification : TokenResponse, INotification {}

public class SearchNewTokensHandler : IRequestHandler<SearchNewTokensRequest>
{
    private readonly ILogger<SearchNewTokensHandler> _logger;
    private readonly ITokensApiClient _tokensApiClient;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    public SearchNewTokensHandler(ILogger<SearchNewTokensHandler> logger, ITokensApiClient tokensApiClient, IMediator mediator, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _tokensApiClient = tokensApiClient;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
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

                using var scope = _serviceProvider.CreateScope();
                var tokenGateway = scope.ServiceProvider.GetRequiredService<ITokenGateway>();
                
                await tokenGateway.AddAsync(new CryptoToken
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

                // await _mediator.Publish(new NewTokenNotification
                // {
                //     Name = response.Name,
                //     Symbol = response.Symbol,
                //     Image = response.ImageUrl,
                //     TokenAddress = response.TokenAddress,
                //     CreatedAtUtc = response.CreatedAtUtc
                // }, cancellationToken);
            });
            
            _tokensApiClient.SubscribeOnNewTransaction(transactions =>
            {
                Console.WriteLine(transactions);
                return Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }
        
        return Task.CompletedTask;
    }
}