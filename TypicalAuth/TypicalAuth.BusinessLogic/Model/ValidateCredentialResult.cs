namespace TypicalAuth.Model;

public class ValidateCredentialResult
{
    public readonly ClientAuthType AuthType;
    public readonly User User;

    private ValidateCredentialResult(ClientAuthType authType, User user)
    {
        AuthType = authType;
        User = user;
    }

    public static ValidateCredentialResult Localy(User user)
        => new ValidateCredentialResult(ClientAuthType.Locally, user);

    public static ValidateCredentialResult Ldap(User user)
       => new ValidateCredentialResult(ClientAuthType.Ldap, user);
}
