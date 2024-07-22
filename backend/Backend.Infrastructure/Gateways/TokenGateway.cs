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
    
    public async Task<IEnumerable<CryptoToken>> GetNewTokens(int offset, int size)
    {
        return await _dataContext.Tokens
            .OrderByDescending(o => o.CreatedAtUtc)
            .Skip(offset)
            .Take(size)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<CryptoToken>> GetTokensOlderThan(DateTime dateTimeUtc)
    {
        return await _dataContext.Tokens
            .Where(o => o.CreatedAtUtc >= dateTimeUtc)
            .OrderByDescending(o => o.CreatedAtUtc)
            .ToListAsync();
    }
    
    public async Task<int> GetTotalSize()
    {
        return await _dataContext.Tokens.CountAsync();
    }
    
    public async Task<CryptoToken?> GetTokenInformation(string address)
    {
        return await _dataContext.Tokens.FirstOrDefaultAsync(o => o.TokenAddress == address);
    }
}