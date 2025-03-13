namespace Laboratory.Backend.Auth.Model;

public class User
{
    public static readonly string AdminUsername = "admin";
    public static readonly string TestUsername = "test";
    public static readonly string TestPassword = "test";
    //public static readonly string AdminDefaultPassword = "C2FA464BDDBE22B5347B459A1148C87F.540177328B430075172CF4EF3D3EDDD6EDB592D9D0DB6D7FEB3C16D41C46A17C"; //"P@ss123";

    public Guid Id { get; set; }
    public required bool Activation { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required bool ChangePasswordRequired { get; set; }

    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Nickname { get; set; }

    public required List<UserRole> Roles { get; set; }

    public string Fullname => $"{Firstname} {Lastname}".Trim();

    public bool LocallyAvailable => !string.IsNullOrWhiteSpace(Password);
    public bool ReadOnly => Username.Equals(AdminUsername, StringComparison.InvariantCultureIgnoreCase);

    // Navigation property for related providers
    public List<UserLdapAccount> LdapAccounts { get; set; } = new List<UserLdapAccount>();
    public List<UserEmail> Emails { get; set; } = new List<UserEmail>();

}
