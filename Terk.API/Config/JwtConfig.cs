namespace Terk.API.Config;

/// <summary>
/// Configuration for JWT
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// Name of section in appsettings.json
    /// </summary>
    public const string SectionName = "JwtConfig";
    
    /// <summary>
    /// Decrypted secret value
    /// </summary>
    public string Secret { get; set; } = null!;
    
    /// <summary>
    /// Name of server that sends JWT-tokens
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Encrypted secret value
    /// </summary>
    public SymmetricSecurityKey SecretKey => new(Encoding.UTF8.GetBytes(Secret));
}