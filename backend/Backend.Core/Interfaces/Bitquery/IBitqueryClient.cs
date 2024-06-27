using Backend.Core.Interfaces.Bitquery.Types;

namespace Backend.Core.Interfaces.Bitquery;

public interface IBitqueryClient : IDisposable
{
    void SubscribeOnNewTokens(Action<NewTokensPayload> callback);
    void SubscribeOnLastTokenTrade(string smartContract, Action<LastTradePayload> callback);
}