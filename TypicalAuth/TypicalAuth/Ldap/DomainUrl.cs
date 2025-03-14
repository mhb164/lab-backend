namespace TypicalAuth.Ldap;

public class DomainUrl
{
    public readonly string Domain;
    public readonly string Url;

    public DomainUrl(string domain, string url)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new ArgumentException($"{nameof(domain)} is required!", nameof(domain));

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException($"{nameof(url)} is required!", nameof(url));

        Domain = domain;
        Url = url;
    }
}