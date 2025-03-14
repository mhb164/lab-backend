namespace Shared.Auth;

public struct ClaimNames
{
    public const string TokenId = "jti";//JwtRegisteredClaimNames.Jti
    public const string UserId = "sub";//JwtRegisteredClaimNames.Sub
    public const string Firstname = "given_name";//JwtRegisteredClaimNames.GivenName
    public const string Lastname = "family_name";//JwtRegisteredClaimNames.FamilyName
    public const string Nickname = "nickname";//JwtRegisteredClaimNames.Nickname
    public const string ExpirationTime = "exp";//JwtRegisteredClaimNames.Exp

    public const string AuthType = "auth_type";
    public const string AuthDetail = "auth_detail";
    public const string AuthTime = "auth_time";

    public const string LocallyAvailable = "locally_available";
    public const string ChangeLocalPasswordRequired = "change_local_password";
    public const string ReadOnly = "read_only";
}
