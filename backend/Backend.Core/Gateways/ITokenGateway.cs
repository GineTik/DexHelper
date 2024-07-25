
using Backend.Domain.Entities;

namespace Backend.Core.Gateways;

public interface ITokenGateway
{
    Task AddAsync(CryptoToken token);
    Task AddRangeAsync(IEnumerable<CryptoToken> tokens);
    Task<bool> IsExists(string address);
    Task<IEnumerable<CryptoToken>> GetNewTokens(int offset, int size);
    Task<IEnumerable<CryptoToken>> GetTokensOlderThan(DateTime dateTimeUtc);
    Task<int> GetTotalSize();
    Task<CryptoToken?> GetTokenInformation(string address);
    Task<IEnumerable<CryptoToken>> GetAllTokens();
}