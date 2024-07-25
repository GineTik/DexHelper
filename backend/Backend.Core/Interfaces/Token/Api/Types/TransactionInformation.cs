namespace Backend.Core.Interfaces.Token.Api.Types;

public class TransactionInformation
{
    public string Type { get; set; } = default!;
    public NewTransactionTokenSide BoughtToken { get; set; } = default!;
    public NewTransactionTokenSide SoldToken { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string Wallet { get; set; } = default!;
}

public class NewTransactionTokenSide
{
    public string Address { get; set; } = default!;
    public string Uri { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
    public decimal PriceUsd { get; set; } = default!;
}