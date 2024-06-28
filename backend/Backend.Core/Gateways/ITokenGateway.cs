
using Backend.Domain.Entities;

namespace Backend.Core.Gateways;

public interface ITokenGateway
{
    Task AddAsync(CryptoToken token);
}