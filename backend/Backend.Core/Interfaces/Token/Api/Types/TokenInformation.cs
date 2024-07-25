using Backend.Domain.Entities;

namespace Backend.Core.Interfaces.Token.Api.Types;

public class TokenInformation
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public string BoundingCurveAddress { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string CreationSignature { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Twitter { get; set; } = default!;
    public string Telegram { get; set; } = default!;
    public string AuthorWallet { get; set; } = default!;

    public CryptoToken ToCryptoToken()
    {
        return new CryptoToken
        {
            Name = Name,
            Symbol = Symbol,
            Description = Description,
            ImageUrl = ImageUrl,
            CreationSignature = CreationSignature,
            CreatedAtUtc = CreatedAtUtc,
            Address = TokenAddress,
            BoundingCurveAddress = BoundingCurveAddress,
            UpdatedAtUtc = CreatedAtUtc,
            Website = Website,
            Twitter = Twitter,
            Telegram = Telegram,
        };
    }
}