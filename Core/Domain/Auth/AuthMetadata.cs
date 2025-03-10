namespace Backend.Auth;

public sealed record AuthMetadata
{
    public readonly bool IsPublic;
    public readonly string? Domain;
    public readonly string? Scope;
    public readonly string? Permission;

    public AuthMetadata(bool isPublic = false, string? domain = null, string? scope = null, string? permission = null)
    {
        IsPublic = isPublic;
        Domain = domain;
        Scope = scope;
        Permission = permission;
    }

    public bool HasAccess(UserPermits? permits)
    {
        if (IsPublic)
            return true;

        if (permits is null)
        {
            //it is private and permits shouldn't be null
            return false;
        }

        if (Domain is null && Scope is null && Permission is null)
        {
            // it is private but token is enough (IsPublic = false)
            return true;
        }

        if (Domain is not null && Scope is null && Permission is null)
        {
            // it is private and domain specific (IsPublic = false, Domain has value)
            return permits.Existed(domain: Domain);
        }

        if (Domain is not null && Scope is not null && Permission is null)
        {
            // it is private and domain-scope specific (IsPublic = false, Domain has value, Scope has value, Permission should be DefaultPermission)
            return permits.Existed(domain: Domain, scope: Scope, permission: UserPermit.DefaultPermission);
        }

        if (Domain is not null && Scope is not null && Permission is not null)
        {
            // it is private and domain-scope-permission specific (IsPublic = false, Domain has value, Scope has value, Permission has value)
            return permits.Existed(domain: Domain, scope: Scope, permission: Permission);
        }

        throw new InvalidOperationException("There is a problem with AuthMetadata!");
    }

    public AuthMetadata NewWith(string? domain = null, string? scope = null, string? permission = null)
    {
        if (domain is null)
            domain = Domain;

        if (scope is null)
            scope = Scope;

        if (permission is null)
            permission = Permission;

        return new AuthMetadata(IsPublic, domain, scope, permission);
    }
}