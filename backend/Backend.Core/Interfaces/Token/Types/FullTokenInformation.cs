namespace Backend.Core.Interfaces.Token.Types;

public class FullTokenInformation
{
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string PriceUsd { get; set; } = default!;
    public string LiquidityUsd { get; set; } = default!;
    public string VolumeUsd { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string CreatedAt { get; set; } = default!;
}