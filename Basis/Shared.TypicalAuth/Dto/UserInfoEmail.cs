namespace Shared.Dto;

public class UserInfoEmail
{
    public UserInfoEmail() { /* Method intentionally left empty.*/ }

    public UserInfoEmail(string? email, bool? verified)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"{nameof(email)} is required!", nameof(email));

        Email = email;
        Verified = verified ?? throw new ArgumentNullException(nameof(verified));
    }

    public string? Email { get; set; }
    public bool? Verified { get; set; }

}
