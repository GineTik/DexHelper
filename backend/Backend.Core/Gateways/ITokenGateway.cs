
using System.Collections;
using Backend.Domain.Entities;

namespace Backend.Core.Gateways;

public interface ITokenGateway
{
    Task AddAsync(CryptoToken token);
    Task<IEnumerable<CryptoToken>> GetTokenList(int offset, int size);
    Task<int> GetTotalSize();
}