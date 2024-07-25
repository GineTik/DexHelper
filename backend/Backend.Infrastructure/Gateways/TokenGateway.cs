using Backend.Core.Gateways;
using Backend.Domain.Entities;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Gateways;

public class TokenGateway : ITokenGateway
{
    private readonly DataContext _dataContext;
    private readonly ILogger<TokenGateway> _logger;

    public TokenGateway(DataContext dataContext, ILogger<TokenGateway> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }

    public async Task AddAsync(CryptoToken token)
    {
        if (await IsExists(token.Address))
            return;

        try
        {
            _dataContext.Tokens.Add(token);
            await _dataContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.ToString());
        }
    }
    
    public async Task AddRangeAsync(IEnumerable<CryptoToken> tokens)
    {
        _dataContext.Tokens.AddRange(tokens);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<bool> IsExists(string address)
    {
        return await _dataContext.Tokens.AnyAsync(o => o.Address == address);
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
        return await _dataContext.Tokens.FirstOrDefaultAsync(o => o.Address == address);
    }
    
    public async Task<IEnumerable<CryptoToken>> GetAllTokens()
    {
        return await _dataContext.Tokens.ToListAsync();
    }
}