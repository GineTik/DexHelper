namespace Backend.Infrastructure.Implementation.Token.TokensApi.Types;

public class BitqueryResponse
{
    public string Type { get; set; } = default!;
    public string? Payload { get; set; } = default!;
}

public class BitqueryResponse<TPayload>
{
    public string Type { get; set; } = default!;
    public TPayload Payload { get; set; } = default!;
}