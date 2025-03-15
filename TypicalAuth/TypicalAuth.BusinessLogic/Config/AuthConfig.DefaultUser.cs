namespace TypicalAuth.Config;

public sealed partial class AuthConfig
{
    public sealed class DefaultUser
    {
        private readonly HashSet<UserRole> _roles = new HashSet<UserRole>();

        public readonly string Username;
        public readonly bool ReadOnly;
        public readonly string Firstname;
        public readonly string Lastname;
        public readonly string Nickname;

        public DefaultUser(string? username, bool @readonly, string? firstname = null, string? lastname = null, string? nickname = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException($"{nameof(username)} is required!", nameof(username));

            if (string.IsNullOrWhiteSpace(firstname) && string.IsNullOrWhiteSpace(lastname))
                throw new ArgumentException($"{nameof(firstname)} or {nameof(lastname)} is required!");

            Username = username;
            ReadOnly = @readonly;
            Firstname = firstname ?? string.Empty;
            Lastname = lastname ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nickname))
                nickname = Username;

            Nickname = nickname;
        }

        public string DefaultPassword { get; private set; } = string.Empty;
        public bool PasswordAlreadyHashed { get; private set; } = false;
        public bool ForceToChangePassword { get; private set; } = false;

        public IEnumerable<UserRole> Roles => _roles;


        public DefaultUser WithPassword(string? defaultPassword, bool alreadyHashed = false, bool forceToChangePassword = false)
        {
            DefaultPassword = defaultPassword ?? string.Empty;
            PasswordAlreadyHashed = alreadyHashed;
            ForceToChangePassword = forceToChangePassword;

            return this;
        }

        public DefaultUser WithRoles(params UserRole[] roles)
        {
            foreach (var role in roles)
                _roles.Add(role);

            return this;
        }
    }
}