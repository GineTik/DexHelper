using Backend.Core.Gateways;
using Backend.Domain.Entities;
using Backend.Infrastructure.EF;

namespace Backend.Infrastructure.Gateways;

public class TokenGateway : ITokenGateway
{
    private readonly DataContext _dataContext;

    public TokenGateway(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(CryptoToken token)
    {
        _dataContext.Tokens.Add(token);
        await _dataContext.SaveChangesAsync();
    }
}