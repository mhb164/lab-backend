namespace Shared.Model;

public class UserPermits
{
    public static string Separator => UserPermit.Separator;
    public readonly IEnumerable<UserPermit> Permits;
    private readonly HashSet<string> _domains;
    private readonly HashSet<string> _fullPermissions;

    public UserPermits(IEnumerable<UserPermit> permits)
    {
        Permits = permits ?? Enumerable.Empty<UserPermit>();

        _domains = [.. Permits.Select(x => x.Domain)];

        _fullPermissions = new HashSet<string>();
        foreach (var permit in Permits)
            foreach (var permission in permit.Permissions)
                _fullPermissions.Add($"{permit.Domain}{Separator}{permit.Scope}{Separator}{permission}");
    }

    public IEnumerable<string> Domains => _domains;
    public IEnumerable<string> FullPermissions => _fullPermissions;

    public bool Existed(string domain)
    {
        return _domains.Contains(domain);
    }

    public bool Existed(string domain, string scope, string permission)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new ArgumentException($"{nameof(domain)} is required!", nameof(domain));
        if (string.IsNullOrWhiteSpace(scope))
            throw new ArgumentException($"{nameof(scope)} is required!", nameof(scope));
        if (string.IsNullOrWhiteSpace(permission))
            throw new ArgumentException($"{nameof(permission)} is required!", nameof(permission));

        var fullPermission = $"{domain}{Separator}{scope}{Separator}{permission}";
        return _fullPermissions.Contains(fullPermission);
    }
}