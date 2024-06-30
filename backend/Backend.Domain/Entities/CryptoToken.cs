using Backend.Domain.Entities.Base;

namespace Backend.Domain.Entities;

public class CryptoToken : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; } = default!;
    public DateTime UpdatedAtUtc { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public string PoolAddress { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Website { get; set; } = default!;
    public string? Twitter { get; set; } = default!;
    public string? Telegram { get; set; } = default!;
}