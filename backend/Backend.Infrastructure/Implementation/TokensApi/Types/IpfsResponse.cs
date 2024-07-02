namespace Backend.Infrastructure.Implementation.TokensApi.Types;

public class IpfsResponse
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Image { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Twitter { get; set; } = default!;
    public string Telegram { get; set; } = default!;
}