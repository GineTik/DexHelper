using Backend.Core.Gateways;
using Backend.Domain.Entities;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IEnumerable<CryptoToken>> GetTokenList(int offset, int size)
    {
        return await _dataContext.Tokens
            .Skip(offset)
            .Take(size)
            .ToListAsync();
    }
    public async Task<int> GetTotalSize()
    {
        return await _dataContext.Tokens.CountAsync();
    }
}