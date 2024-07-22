using Backend.Domain.Entities;

namespace Backend.Core.Interfaces.Token.TokensApi.Types;

public class NewToken
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public string PoolAddress { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string TransactionSignature { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Twitter { get; set; } = default!;
    public string Telegram { get; set; } = default!;
    public string Wallet { get; set; } = default!;
}