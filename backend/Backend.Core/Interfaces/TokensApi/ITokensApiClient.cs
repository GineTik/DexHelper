using Backend.Core.Interfaces.TokensApi.Types;

namespace Backend.Core.Interfaces.TokensApi;

public interface ITokensApiClient : IDisposable
{
    void SubscribeOnNewTokens(Func<NewToken, Task> callback);
}