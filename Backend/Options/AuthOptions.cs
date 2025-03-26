namespace Laboratory.Backend.Options;

public class AuthOptions
{
    public const string ConfigName = "Auth";

    public List<AuthOptionsDomain>? Domains { get; set; }
}
