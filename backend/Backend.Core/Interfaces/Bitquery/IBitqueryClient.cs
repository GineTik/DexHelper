using Backend.Core.Interfaces.Bitquery.Types;

namespace Backend.Core.Interfaces.Bitquery;

public interface IBitqueryClient : IDisposable
{
    void SubscribeOnNewTokens(Func<NewTokensPayload, Task> callback);
    void SubscribeOnLastTokenTrade(string smartContract, Func<LastTradePayload, Task> callback);
}