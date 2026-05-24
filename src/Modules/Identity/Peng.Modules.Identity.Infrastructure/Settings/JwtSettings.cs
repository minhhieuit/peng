namespace Peng.Modules.Identity.Infrastructure.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; init; } = default!;
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public int ExpirationSeconds { get; init; } = 3600;
}
