namespace Terk.API.Extensions;

public static class JwtExtensions
{
    public static int? ExtractId(this string authorizationToken) => authorizationToken.ExtractJwtToken().ExtractId();

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

    public static int? ExtractId(this JwtSecurityToken jwt)
    {
        var idClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        var success = int.TryParse(idClaim?.Value, out var id);
        return success ? id : null;
    }
}