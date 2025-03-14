namespace Shared.Auth;

public class ClientUser
{
    public static readonly string HttpContextKey = "ClientUser";

    public readonly ClientToken Token;
    public readonly ClientAuth Auth;

    public readonly Guid Id;
    public readonly string Firstname;
    public readonly string Lastname;
    public readonly string Nickname;

    public readonly bool LocallyAvailable;
    public readonly bool ChangeLocalPasswordRequired;
    public readonly bool ReadOnly;

    public readonly UserPermits Permits;


    public ClientUser(ClientToken? token, ClientAuth? auth,
        Guid? id, string? firstname, string? lastname, string? nickname,
        bool locallyAvailable, bool changeLocalPasswordRequired, bool readOnly, UserPermits permits)
    {
        if (token is null)
            throw new ArgumentNullException(nameof(token));

        if (auth is null)
            throw new ArgumentNullException(nameof(auth));

        if (id is null)
            throw new ArgumentNullException(nameof(id));

        if (firstname is null)
            throw new ArgumentNullException(nameof(firstname));
        if (string.IsNullOrWhiteSpace(lastname))
            throw new ArgumentException($"{nameof(lastname)} is required!", nameof(lastname));
        if (string.IsNullOrWhiteSpace(nickname))
            throw new ArgumentException($"{nameof(nickname)} is required!", nameof(nickname));

        if (permits is null)
            throw new ArgumentNullException(nameof(permits));

        Token = token;
        Auth = auth;
        Id = id.Value;
        Firstname = firstname!;
        Lastname = lastname!;
        Nickname = nickname!;

        ChangeLocalPasswordRequired = changeLocalPasswordRequired;
        LocallyAvailable = locallyAvailable;
        ReadOnly = readOnly;

        Permits = permits;
    }

    public string Fullname => $"{Firstname} {Lastname}".Trim();

}