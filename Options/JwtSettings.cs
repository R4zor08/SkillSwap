namespace SkillSwap.Web.Options;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "SkillSwap";
    public string Audience { get; set; } = "SkillSwap.Api";
    public int ExpirationMinutes { get; set; } = 60;
}
