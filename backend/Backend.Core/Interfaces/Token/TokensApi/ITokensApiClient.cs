using Backend.Core.Interfaces.Token.TokensApi.Types;

namespace Backend.Core.Interfaces.Token.TokensApi;

public interface ITokensApiClient : IDisposable
{
    void SubscribeOnNewTokens(Func<NewToken, Task> callback);
    void SubscribeOnNewTransaction(Func<IEnumerable<NewTransaction>, Task> callback);
}