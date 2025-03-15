namespace TypicalAuth.Config;

public sealed partial class AuthConfig
{
    private readonly List<DefaultUser> _defaultUsers = new List<DefaultUser>();
    private readonly Dictionary<string, DefaultUser> _readOnlyUsers = new Dictionary<string, DefaultUser>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<UserRole, List<UserPermit>> _rolesPermits = new Dictionary<UserRole, List<UserPermit>>();

    private Func<Guid, Guid, (string TokenId, string UserId)> _toJWTFunc;
    private Func<string, string, (Guid? TokenId, Guid UserId)> _fromJWTFunc;

    public IEnumerable<DefaultUser> DefaultUsers => _defaultUsers;

    public bool IsReadOnly(string username)
        => _readOnlyUsers.ContainsKey(username);

    public void Add(DefaultUser defaultUser)
    {
        _defaultUsers.Add(defaultUser);

        if (defaultUser.ReadOnly)
            _readOnlyUsers.Add(defaultUser.Username, defaultUser);
    }

    public void ConfigureToJWT(Func<Guid, Guid, (string TokenId, string UserId)> toJWTFunc)
        => _toJWTFunc = toJWTFunc;

    public void ConfigureFromJWT(Func<string, string, (Guid? TokenId, Guid UserId)> fromJWTFunc)
        => _fromJWTFunc = fromJWTFunc;

    public (string TokenId, string UserId) ToJWT(Guid tokenId, Guid userId)
    {
        if (_toJWTFunc is not null)
            return _toJWTFunc.Invoke(tokenId, userId);

        return new(tokenId.ToString("N"), userId.ToString("N"));
    }

    public (Guid? TokenId, Guid UserId) FromJWT(string? tokenIdValue, string? userIdValue)
    {
        if (string.IsNullOrWhiteSpace(tokenIdValue))
            throw new ArgumentException($"{nameof(tokenIdValue)} is required!", nameof(tokenIdValue));
        if (string.IsNullOrWhiteSpace(userIdValue))
            throw new ArgumentException($"{nameof(userIdValue)} is required!", nameof(userIdValue));

        if (_fromJWTFunc is not null)
            return _fromJWTFunc.Invoke(tokenIdValue, userIdValue);

        Guid? tokenId = null;
        if (Guid.TryParse(tokenIdValue, out var parsedTokenId))
            tokenId = parsedTokenId;

        if (!Guid.TryParse(userIdValue, out var userId))
            userId = Guid.Empty;

        return new(tokenId, userId);
    }

    public void Add(UserRole role, params UserPermit[] userPermits)
    {
        if (!_rolesPermits.ContainsKey(role))
            _rolesPermits.Add(role, new List<UserPermit>());

        _rolesPermits[role].AddRange(userPermits);
    }
    public IEnumerable<UserPermit> GetPermits(UserRole role)
    {
        if (!_rolesPermits.TryGetValue(role, out var rolePermits))
            return Enumerable.Empty<UserPermit>();

        return rolePermits;
    }
}