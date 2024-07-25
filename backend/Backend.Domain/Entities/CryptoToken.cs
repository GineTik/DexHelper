using System.Collections;
using System.ComponentModel.DataAnnotations;
using Backend.Domain.Entities.Base;

namespace Backend.Domain.Entities;

public class CryptoToken
{
    [Key]
    public string Address { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
    public DateTime UpdatedAtUtc { get; set; } = default!;
    public string CreationSignature { get; set; } = default!;
    public string BoundingCurveAddress { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Website { get; set; } = default!;
    public string? Twitter { get; set; } = default!;
    public string? Telegram { get; set; } = default!;

    public ICollection<Transaction> Transactions { get; set; } = default!;
}