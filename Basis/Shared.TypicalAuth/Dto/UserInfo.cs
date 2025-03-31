namespace Shared.Dto;

public class UserInfo
{
    public UserInfo() { /* Method intentionally left empty.*/ }

    public UserInfo(string? username, string? firstname, string? lastname, bool? locallyAvailable, bool? readOnly , List<string>? ldapAccounts, List<UserInfoEmail>? emails)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException($"{nameof(username)} is required!", nameof(username));
        if (string.IsNullOrWhiteSpace(firstname) && string.IsNullOrWhiteSpace(lastname))
            throw new ArgumentException($"{nameof(username)} or {nameof(lastname)} is required!");

        Username = username;
        Firstname = firstname;
        Lastname = lastname;
        LocallyAvailable = locallyAvailable ?? throw new ArgumentNullException(nameof(locallyAvailable)); 
        ReadOnly = readOnly ?? throw new ArgumentNullException(nameof(readOnly)); 
        LdapAccounts = ldapAccounts ?? new List<string>();
        Emails = emails ?? new List<UserInfoEmail>();
    }

    public string? Username { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public bool? LocallyAvailable { get; set; }
    public bool? ReadOnly { get; set; }
    public List<string> LdapAccounts { get; set; }
    public List<UserInfoEmail> Emails { get; set; }
}
