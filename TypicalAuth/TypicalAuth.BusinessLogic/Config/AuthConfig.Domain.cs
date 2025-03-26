namespace TypicalAuth.Config;

public sealed partial class AuthConfig
{
    public sealed class Domain
    {
        public readonly string Displayname;
        public readonly string Name;

        public Domain(string? displayname, string? name)
        {
            if (string.IsNullOrWhiteSpace(displayname))
                throw new ArgumentException($"{nameof(displayname)} is required!", nameof(displayname));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{nameof(name)} is required!", nameof(name));

            Displayname = displayname.Trim();
            Name = name.Trim();
        }
    }
}