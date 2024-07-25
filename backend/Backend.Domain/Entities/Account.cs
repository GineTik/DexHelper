using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities;

public class Account
{
    [Key]
    public string Wallet { get; set; } = default!;
    public string UserName { get; set; } = default!;

    public ICollection<Transaction> Transactions { get; set; } = default!;
}