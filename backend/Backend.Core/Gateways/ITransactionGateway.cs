using Backend.Domain.Entities;

namespace Backend.Core.Gateways;

public interface ITransactionGateway
{
    Task AddRangeAsync(IEnumerable<Transaction> transactions);
    Task AddInNotExistsAsync(Transaction transaction);
}