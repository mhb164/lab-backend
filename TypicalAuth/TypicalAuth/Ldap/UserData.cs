namespace TypicalAuth.Ldap;

public class UserData
{
    private const string UserPrincipalNameKey = "userprincipalname";
    private const string GivenNameKey = "givenName";
    private const string SurnameKey = "sn";
    private const string EmailKey = "mail";

    public readonly string UserPrincipalName;
    public readonly string GivenName;
    public readonly string Surname;
    public readonly string Email;

    public UserData(Dictionary<string, string> allData)
        : this(allData[UserPrincipalNameKey], allData[GivenNameKey], allData[SurnameKey], allData[EmailKey]) { }

    public UserData(string userPrincipalName, string givenName, string surname, string email)
    {
        if (string.IsNullOrWhiteSpace(userPrincipalName))
            throw new ArgumentException($"{nameof(userPrincipalName)} is required!", nameof(userPrincipalName));
        if (string.IsNullOrWhiteSpace(givenName))
            throw new ArgumentException($"{nameof(givenName)} is required!", nameof(givenName));
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException($"{nameof(surname)} is required!", nameof(surname));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"{nameof(email)} is required!", nameof(email));

        UserPrincipalName = userPrincipalName;
        GivenName = givenName;
        Surname = surname;
        Email = email;
    }
    public override string ToString()
            => $"{UserPrincipalName}: [GivenName:{GivenName}][Surname:{Surname}][Email:{Email}]";
}