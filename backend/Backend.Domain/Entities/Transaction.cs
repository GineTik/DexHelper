using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Domain.Entities.Base;

namespace Backend.Domain.Entities;

public enum TransactionType
{
    Buy = 1,
    Sell = 2,
}

public class Transaction
{
    [Key]
    public string Signature { get; set; } = default!;
    public TransactionType Type { get; set; }
    public decimal BoughtTokenAmount { get; set; } = default!;
    public decimal SoldTokenAmount { get; set; } = default!;
    public decimal BoughtTokenPriceUsd { get; set; } = default!;
    public decimal SoldTokenPriceUsd { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;

    [ForeignKey("Tokens")] 
    public string CryptoTokenAddress { get; set; } = default!;
    
    [ForeignKey(nameof(Account))]
    public string AccountWallet { get; set; } = default!;
}