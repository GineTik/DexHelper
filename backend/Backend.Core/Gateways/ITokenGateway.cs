
using System.Collections;
using Backend.Domain.Entities;

namespace Backend.Core.Gateways;

public interface ITokenGateway
{
    Task AddAsync(CryptoToken token);
    Task<IEnumerable<CryptoToken>> GetNewTokens(int offset, int size);
    Task<IEnumerable<CryptoToken>> GetTokensOlderThan(DateTime dateTimeUtc);
    Task<int> GetTotalSize();
}