﻿namespace Backend.Domain.Options;

public class BitqueryOptions
{
    public const string Name = "Bitquery";

    public string SubscribeUrl { get; set; } = default!;
    public string QueryUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
}
