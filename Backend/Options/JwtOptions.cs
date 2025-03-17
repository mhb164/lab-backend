namespace Laboratory.Backend.Options;

public class JwtOptions
{
    public const string ConfigName = "Jwt";

    public string? Issuer { get; set; }
    public string? Secret { get; set; }
    public double? LifetimeInMinutes { get; set; }
    public double? RefreshDays { get; set; }

    public static JwtConfig ToModel(JwtOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new JwtConfig(options.Issuer, options.Secret, options?.LifetimeInMinutes, options?.RefreshDays);
    }
}