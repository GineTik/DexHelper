namespace Backend.Core.Futures.Token.Types;

public class TokenResponse
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Image { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
}