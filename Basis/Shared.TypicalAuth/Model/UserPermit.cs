namespace Shared.Model;

public class UserPermit
{
    public const string Separator = ":";
    public static readonly string[] SeparatorArray = new string[] { Separator };
    public const string PermitsSeparator = ",";
    public static readonly string[] PermitsSeparatorArray = new string[] { PermitsSeparator };
    public const string ClaimTypePrefix = "PERMIT#";
    public const string DefaultPermission = "view";

    public readonly string Domain;
    public readonly string Scope;
    private readonly HashSet<string> _permissions;

    public UserPermit(string domain, string scope, IEnumerable<string> permissions)
    {
        Domain = domain;
        Scope = scope;

        _permissions = [DefaultPermission, .. permissions];
    }

    public string Name => $"{Domain}{Separator}{Scope}";
    public string ClaimType => $"{ClaimTypePrefix}{Name}";
    public string PermissionsAsText => string.Join(PermitsSeparator, _permissions);
    public IEnumerable<string> Permissions => _permissions;

    public override string ToString()
        => $"[{Name}:{PermissionsAsText}]";

    public static UserPermit? FromClaim(string? claimType, string? permitsAsText)
    {
        if (string.IsNullOrWhiteSpace(claimType) ||
            string.IsNullOrWhiteSpace(permitsAsText))
            return default;

        if (!claimType!.StartsWith(ClaimTypePrefix))
            return default;

        var segments = claimType.Substring(ClaimTypePrefix.Length).Split(SeparatorArray, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 2)
            return default;

        if (string.IsNullOrWhiteSpace(segments[0]) || string.IsNullOrWhiteSpace(segments[1]))
            return default;

        var permits = permitsAsText!.Split(PermitsSeparatorArray, StringSplitOptions.RemoveEmptyEntries);

        return new UserPermit(domain: segments[0], scope: segments[1], permits);
    }
}