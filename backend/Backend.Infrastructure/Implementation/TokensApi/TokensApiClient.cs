using System.Text.Json;
using Backend.Core.Interfaces.Token.TokensApi;
using Backend.Core.Interfaces.Token.TokensApi.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.TokensApi.Types;
using Backend.Infrastructure.Implementation.TokensApi.Types.PumpPortal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Implementation.TokensApi;

public class TokensApiClient : AbstractTokenApiClient, ITokensApiClient
{
    private readonly ILogger<TokensApiClient> _logger;
    private readonly PumpPortalFunOptions _pumpPortalFunOptions;
    
    public TokensApiClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<TokensApiClient> logger, IOptions<PumpPortalFunOptions> pumpPortalFunOptions) 
        : base(bitqueryOptions, logger, pumpPortalFunOptions)
    {
        _logger = logger;
        _pumpPortalFunOptions = pumpPortalFunOptions.Value;
    }
    
    public void SubscribeOnNewTokens(Func<NewToken, Task> callback)
    {
        SubscribeOnPumpPortal<NewTokenResponse>(
            async (newToken) =>
            {
                var httpClient = new HttpClient();
                var stringIpfsResponse = await httpClient.GetStringAsync($"{_pumpPortalFunOptions.TokenInfoUrl}?ca={newToken.Mint}");
                using var doc = JsonDocument.Parse(stringIpfsResponse);
                var root = doc.RootElement;
                var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(root.GetProperty("data").GetRawText(), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (ipfsResponse == null)
                    return;
                
                await callback(new NewToken
                {
                    Name = ipfsResponse.Name,
                    Symbol = ipfsResponse.Symbol,
                    Description = ipfsResponse.Description,
                    CreatedAtUtc = DateTime.UtcNow,
                    ImageUrl = ipfsResponse.Image,
                    TransactionSignature = newToken.Signature,
                    TokenAddress = newToken.Mint,
                    PoolAddress = newToken.BondingCurveKey,
                    Website = ipfsResponse.Website,
                    Twitter = ipfsResponse.Twitter,
                    Telegram = ipfsResponse.Telegram,
                });
            },
            new
            {
                method = "subscribeNewToken"
            }
        );
    }
}