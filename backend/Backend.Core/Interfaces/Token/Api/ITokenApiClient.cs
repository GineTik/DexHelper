using Backend.Core.Interfaces.Token.Api.Types;

namespace Backend.Core.Interfaces.Token.Api;

public interface ITokenApiClient : IDisposable
{
    void SubscribeOnNewTokens(Func<TokenInformation, Task> callback);
    Task<TokenInformation?> GetToken(string address);
    Task<IEnumerable<TokenInformation>> GetAllTokens(IEnumerable<string> exceptAddresses);
}