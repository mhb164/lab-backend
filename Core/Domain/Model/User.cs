using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Model;

public class UserLdapAccount
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Username { get; set; }

    // Navigation property
    public User User { get; set; } 
}

public class User
{
    public static readonly string AdminUsername = "admin";
    //public static readonly string AdminDefaultPassword = "C2FA464BDDBE22B5347B459A1148C87F.540177328B430075172CF4EF3D3EDDD6EDB592D9D0DB6D7FEB3C16D41C46A17C"; //"P@ss123";

    public Guid Id { get; set; }
    public required bool Activation { get; set; }
    public required string Username { get; set; }
    public required string LocalPassword { get; set; }
    public bool ChangeLocalPasswordRequired { get; set; }

    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required string Nickname { get; set; }

    public required List<UserRole> Roles { get; set; }

    public string Fullname => $"{Firstname} {Lastname}".Trim();

    public bool LocallyAvailable => !string.IsNullOrWhiteSpace(LocalPassword);
    public bool ReadOnly => Username.Equals(AdminUsername, StringComparison.InvariantCultureIgnoreCase);

    // Navigation property for related providers
    public List<UserLdapAccount> LdapUsernames { get; set; } = new List<UserLdapAccount>();

}


public enum UserRole
{
    SuperAdmin = 1,
    Admin = 2,
    Operator = 3,
    View = 4
}