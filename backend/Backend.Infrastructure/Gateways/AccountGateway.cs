using Backend.Core.Gateways;
using Backend.Domain.Entities;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Gateways;

public class AccountGateway : IAccountGateway
{
    private readonly DataContext _dataContext;
    public AccountGateway(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddIfNotExistsAsync(string wallet)
    {
        if (await _dataContext.Accounts.AnyAsync(o => o.Wallet == wallet))
            return;
        
        _dataContext.Accounts.Add(new Account
        {
            Wallet = wallet,
            UserName = wallet
        });
        await _dataContext.SaveChangesAsync();
    }
}