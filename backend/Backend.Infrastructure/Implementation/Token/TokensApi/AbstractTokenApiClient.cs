﻿using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Backend.Core.Interfaces.Token.TokensApi.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.Token.TokensApi.Types.PumpPortal;
using Backend.Infrastructure.Implementation.Token.TokensApi.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Websocket.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Backend.Infrastructure.Implementation.Token.TokensApi;

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
    
    protected void SubscribeOnBitquery(Func<dynamic, Task> callback, string query, [CallerMemberName] string name = "nothing")
    {
        if (_websocketClients.ContainsKey(name))
            return;
        
        var websocketClient = CreateWebsocketClient();

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
            var response = ParseJson(jsonResponse.Text);
            
            switch (response?.Type)
            {
                case "connection_ack":
                    websocketClient.Send(JsonSerializer.Serialize(new
                    {
                        type = "start", 
                        id = "1", 
                        payload = new
                        {
                            query = query                            
                        }
                    }));
                    break;
                case @"""data""":
                    var data = GetData(jsonResponse.ToString());
                    //callback(JsonSerializer.Deserialize<T>(data) ?? new T());
                    callback(JsonConvert.DeserializeObject(data) ?? throw new InvalidOperationException("DeserializeObject(data) return null"));
                    break;
            };
        });
        
        websocketClient.Send(JsonSerializer.Serialize(new
        {
            type = "connection_init",
            payload = new { }
        }));
        websocketClient.Start();
        _websocketClients[name] = websocketClient;
    }
    
    private WebsocketClient CreateWebsocketClient()
    {
        var factory = new Func<ClientWebSocket>(() =>
        {
            var client = new ClientWebSocket();
            client.Options.SetRequestHeader("Content-Type", "application/json");
            client.Options.SetRequestHeader("X-API-KEY", _bitqueryOptions.ApiKey);
            client.Options.SetRequestHeader("Authorization", $"Bearer {_bitqueryOptions.AccessToken}");
            client.Options.AddSubProtocol("graphql-ws");
            return client;
        });
        var url = new Uri(_bitqueryOptions.Url + $"?token={_bitqueryOptions.AccessToken}");
        var client = new WebsocketClient(url, factory);
        return client;
    }
    private static BitqueryResponse? ParseJson(string? json)
    {
        json ??= "{}";
        
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var type = root.GetProperty("type").GetRawText();
            var payload = (type == "data" 
                ? root.GetProperty("payload").GetProperty("data") 
                : root.GetProperty("payload")
            ).GetRawText();
            return new BitqueryResponse
            {
                Type = type,
                Payload = payload
            };
        }
        catch
        {
            return JsonSerializer.Deserialize<BitqueryResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
    private static string GetData(string bitqueryJsonResponse)
    {
        using var doc = JsonDocument.Parse(bitqueryJsonResponse);
        var root = doc.RootElement;
        return root.GetProperty("payload").GetProperty("data").GetRawText();
    }
}