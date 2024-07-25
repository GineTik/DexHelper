namespace Backend.Constants;

public static class SignalRGroupsConstants
{
    public static string TrackTokenTransaction(string address)
    {
        return $"[Token]{address}";
    }
}