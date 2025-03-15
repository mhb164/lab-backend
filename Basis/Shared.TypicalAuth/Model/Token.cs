namespace Shared.Model;

public class Token
{
    public Token() : this(string.Empty, string.Empty)
    {
    }

    public Token(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
