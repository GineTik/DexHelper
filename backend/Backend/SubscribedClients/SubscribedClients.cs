using System.Net.WebSockets;
using Backend.Core.Interfaces.SubscribedClients;
using Backend.Domain.Entities;

namespace Backend.SubscribedClients;

public class SubscribedClients : ISubscribedClients
{
    private readonly IDictionary<SubscribeTypes, ICollection<ClientWebSocket>> _clientsByTypes = new Dictionary<SubscribeTypes, ICollection<ClientWebSocket>>();
    
    public void AddNewClient(SubscribeTypes type, ClientWebSocket client)
    {
        _clientsByTypes[type] ??= Array.Empty<ClientWebSocket>();
        _clientsByTypes[type].Add(client);
    }

    public void SendNewTokens(IEnumerable<CryptoToken> tokens)
    {
        throw new NotImplementedException();
    }
}