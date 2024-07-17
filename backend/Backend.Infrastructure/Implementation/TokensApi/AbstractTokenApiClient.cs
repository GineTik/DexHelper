using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Backend.Core.Interfaces.Token.TokensApi.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.TokensApi.Types.PumpPortal;
using Backend.Infrastructure.Implementation.TokensApi.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Websocket.Client;

namespace Backend.Infrastructure.Implementation.TokensApi;

public abstract class AbstractTokenApiClient : IDisposable
{
    private readonly IDictionary<string, WebsocketClient> _websocketClients;
    private readonly BitqueryOptions _bitqueryOptions;
    private readonly ILogger<TokensApiClient> _logger;
    private readonly PumpPortalFunOptions _pumpPortalFunOptions;

    protected AbstractTokenApiClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<TokensApiClient> logger, IOptions<PumpPortalFunOptions> pumpPortalFunOptions)
    {
        _logger = logger;
        _pumpPortalFunOptions = pumpPortalFunOptions.Value;
        _bitqueryOptions = bitqueryOptions.Value;
        _websocketClients = new Dictionary<string, WebsocketClient>();
    }
    
    public void Dispose()
    {
        foreach (var client in _websocketClients)
        {
            client.Value.Stop(WebSocketCloseStatus.NormalClosure, "Dispose BitqueryClient");
            client.Value.Dispose();
        }
    }
    
    protected void SubscribeOnPumpPortal<T>(Func<T, Task> callback, object payload, [CallerMemberName] string name = "nothing")
        where T : new()
    {
        if (_websocketClients.ContainsKey(name))
            return;
        
        var url = new Uri(_pumpPortalFunOptions.StreamingUrl);
        var websocketClient = new WebsocketClient(url);
        websocketClient.Send(JsonSerializer.Serialize(payload));

        websocketClient.ReconnectTimeout = null; // off
        websocketClient.DisconnectionHappened.Subscribe(info =>
        {
            _logger.LogError($"Disconnection happened, type: {info.Type}, exception: {info.Exception}");
        });
        websocketClient.ReconnectionHappened.Subscribe(info =>
        {
            _logger.LogWarning("Reconnection happened, type: " + info.Type);
        });
        websocketClient.MessageReceived.Subscribe(jsonResponse =>
        {
            _logger.LogInformation("Message received: " + jsonResponse);
            
            if (jsonResponse.ToString().StartsWith("{\"message\":"))
                return;
            
            var response = JsonSerializer.Deserialize<T>(jsonResponse.ToString(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            if (response != null)
                callback(response);
        });

        websocketClient.Start();
        _websocketClients[name] = websocketClient;
    }
}