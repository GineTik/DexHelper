namespace Backend.Core.Interfaces.Token.TokensApi.Types;

public class NewTransaction
{
    public string Type { get; set; } = default!;
    public string BoughtTokenMintAddress { get; set; } = default!;
    public string SoldTokenMinAddress { get; set; } = default!;
    public decimal BoughtTokenAmount { get; set; } = default!;
    public decimal SoldTokenAmount { get; set; } = default!;
    public decimal BoughtTokenPriceUsd { get; set; } = default!;
    public decimal SoldTokenPriceUsd { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string Wallet { get; set; } = default!;
}