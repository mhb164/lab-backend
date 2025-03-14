namespace Shared.Auth;

public class ClientAuth
{
    public readonly ClientAuthType Type;
    public readonly string Detail;
    public readonly DateTime Time;

    public ClientAuth(string? typeText, string? detail, DateTime? time)
    {
        if (string.IsNullOrWhiteSpace(typeText))
            throw new ArgumentException($"{nameof(typeText)} is required!", nameof(typeText));

        if (!Enum.TryParse<ClientAuthType>(typeText, true, out var type))
        {
            throw new ArgumentException($"Invalid {nameof(typeText)} ({typeText})", nameof(typeText));
        }

        if (string.IsNullOrWhiteSpace(detail))
            throw new ArgumentException($"{nameof(detail)} is required!", nameof(detail));

        if (time is null)
            throw new ArgumentNullException(nameof(time));

        Type = type;
        Detail = detail!;
        Time = time.Value;
    }
}
