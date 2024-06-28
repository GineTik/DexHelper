namespace Backend.Domain.Entities;

public class CryptoToken
{
    public DateTime CreatedAt { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public string PoolAddress { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public IEnumerable<Social> Socials { get; set; } = default!;
}