namespace Backend.Domain.Options;

public class PumpPortalFunOptions
{
    public const string Name = "PompPortalFun";
    
    public string StreamingUrl { get; set; } = default!;
    public string TokenInfoUrl { get; set; } = default!;
}