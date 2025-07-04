namespace UniformesSystem.API.Models;

public class AuthSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
