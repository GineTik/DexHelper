namespace Backend.Infrastructure.Implementation.Token.TokensApi.Types.PumpPortal;

public class NewTokenResponse
{
    public string Signature { get; set; } = default!;
    public string Mint { get; set; } = default!;
    public string TraderPublicKey { get; set; } = default!;
    public string TxType { get; set; } = default!;
    public decimal InitialBuy { get; set; } = default!;
    public string BondingCurveKey { get; set; } = default!;
    public decimal VTokensInBondingCurve { get; set; } = default!;
    public decimal VSolInBondingCurve { get; set; } = default!;
    public decimal MarketCapSol { get; set; } = default!;
    public string Uri { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
}