using System.Text.Json;
using Backend.Core.Futures.Token.Types;
using Backend.Core.Gateways;
using Backend.Core.Interfaces.Token.Api;
using Backend.Core.Interfaces.Token.Api.Types;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.Token.Trackers.TrackNewToken;


public record TrackNewTokenRequest() : IRequest;

public class NewTokenNotification : CryptoToken, INotification {}

internal class TrackNewTokenHandler : IRequestHandler<TrackNewTokenRequest>
{
    private readonly ILogger<TrackNewTokenHandler> _logger;
    private readonly ITokenApiClient _tokenApiClient;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITokenGateway _globalTokenGateway;

    public TrackNewTokenHandler(ILogger<TrackNewTokenHandler> logger, ITokenApiClient tokenApiClient, IMediator mediator, IServiceProvider serviceProvider, ITokenGateway globalTokenGateway)
    {
        _logger = logger;
        _tokenApiClient = tokenApiClient;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
        _globalTokenGateway = globalTokenGateway;
    }

    public Task Handle(TrackNewTokenRequest newTokenRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Search new tokens");

        try
        {
            _tokenApiClient.SubscribeOnNewTokens(async token =>
            {
                _logger.LogInformation("new token: " + JsonSerializer.Serialize(token));

                // TODO: filtration soon
                // var isValid = await _tokenFilter.Filter(fullTokenInformation);
                // if (isValid == false)
                //     return;

                using var scope = _serviceProvider.CreateScope();
                var tokenGateway = scope.ServiceProvider.GetRequiredService<ITokenGateway>();
                
                var convertedToken = ConvertToken(token);
                await tokenGateway.AddAsync(convertedToken);
                await _mediator.Publish(convertedToken, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }

        return Task.CompletedTask;
    }

    private static NewTokenNotification ConvertToken(TokenInformation token)
    {
        return new NewTokenNotification
        {
            Name = token.Name,
            Symbol = token.Symbol,
            Description = token.Description,
            ImageUrl = token.ImageUrl,
            CreationSignature = token.CreationSignature,
            CreatedAtUtc = token.CreatedAtUtc,
            Address = token.TokenAddress,
            BoundingCurveAddress = token.BoundingCurveAddress,
            UpdatedAtUtc = token.CreatedAtUtc,
            Website = token.Website,
            Twitter = token.Twitter,
            Telegram = token.Telegram,
        };
    }
}