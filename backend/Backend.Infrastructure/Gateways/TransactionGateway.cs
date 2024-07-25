using Backend.Core.Gateways;
using Backend.Domain.Entities;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Gateways;

public class TransactionGateway : ITransactionGateway
{
    private readonly DataContext _dataContext;
    
    public TransactionGateway(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> transactions)
    {
        _dataContext.Transactions.AddRange(transactions);
        await _dataContext.SaveChangesAsync();
    }
    
    public async Task AddInNotExistsAsync(Transaction transaction)
    {
        if (await _dataContext.Transactions.AnyAsync(o => o.Signature == transaction.Signature))
            return;
        
        _dataContext.Transactions.Add(transaction);
        await _dataContext.SaveChangesAsync();
    }
}