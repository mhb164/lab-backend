using Backend.Config;

namespace Backend.Options;

public class JwtOptions
{
    public const string ConfigName = "Jwt";

    public string? Issuer { get; set; }
    public string? Secret { get; set; }
    public int? LifetimeInMinutes { get; set; }

    public static JwtConfig ToModel(JwtOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new JwtConfig(options.Issuer, options.Secret, options?.LifetimeInMinutes);
    }
}