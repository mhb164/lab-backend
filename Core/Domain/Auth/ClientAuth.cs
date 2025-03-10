namespace Backend.Auth;

public class ClientAuth
{
    public readonly AuthType Type;
    public readonly string Username;
    public readonly DateTime Time;

    public ClientAuth(string? typeText, string? username, DateTime? time)
    {
        if (string.IsNullOrWhiteSpace(typeText))
            throw new ArgumentException($"{nameof(typeText)} is required!", nameof(typeText));

        if (!Enum.TryParse<AuthType>(typeText, true, out var type))
        {
            throw new ArgumentException($"Invalid {nameof(typeText)} ({typeText})", nameof(typeText));
        }

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException($"{nameof(username)} is required!", nameof(username));

        if (time is null)
            throw new ArgumentNullException(nameof(time));

        Type = type;
        Username = username!;
        Time = time.Value;
    }
}
