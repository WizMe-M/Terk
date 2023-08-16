namespace Terk.API.Extensions;

public static class JwtExtensions
{
    /// <summary>
    /// Extracts JWT bearer token from string and parses <see cref="JwtRegisteredClaimNames.Sub"/> as ID (int) value
    /// </summary>
    /// <param name="authorizationToken">String that contains JWT bearer token</param>
    /// <returns>Int value of ID either null if unable to parse ID</returns>
    public static int? ExtractId(this string authorizationToken) => authorizationToken.ExtractJwtToken().ExtractId();

    /// <summary>
    /// Extracts JWT token from string
    /// </summary>
    /// <param name="authorizationToken">String that contains JWT bearer token</param>
    /// <returns>JWT token</returns>
    public static JwtSecurityToken ExtractJwtToken(this string authorizationToken)
    {
        var tokenString = authorizationToken;
        if (authorizationToken.Contains(JwtBearerDefaults.AuthenticationScheme,
                StringComparison.CurrentCultureIgnoreCase))
        {
            var prefixLength = $"{JwtBearerDefaults.AuthenticationScheme} ".Length;
            tokenString = authorizationToken[prefixLength..];
        }

        var jwtHandler = new JwtSecurityTokenHandler();
        return jwtHandler.ReadJwtToken(tokenString);
    }

    /// <summary>
    /// Parses value of ID from JWT token
    /// </summary>
    /// <param name="jwt">JWT token</param>
    /// <returns>Int value of ID either null if unable to parse ID</returns>
    public static int? ExtractId(this JwtSecurityToken jwt)
    {
        var idClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        var success = int.TryParse(idClaim?.Value, out var id);
        return success ? id : null;
    }
}