using System.Net.WebSockets;
using Backend.Domain.Entities;

namespace Backend.Core.Interfaces.SubscribedClients;

public interface ISubscribedClients
{
    void AddNewClient(SubscribeTypes type, ClientWebSocket client);
    void SendNewTokens(IEnumerable<CryptoToken> tokens);
}