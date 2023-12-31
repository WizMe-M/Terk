﻿namespace Terk.API.Controllers;

/// <summary>
/// Controller with authentication endpoints
/// </summary>
[Route("api/auth")]
public class AuthController : DbController
{
    private readonly ILogger<AuthController> _logger;
    private readonly JwtConfig _jwtConfig;

    public AuthController(ILogger<AuthController> logger, IOptions<JwtConfig> jwtConfigOptions, TerkDbContext context) :
        base(context)
    {
        _logger = logger;
        _jwtConfig = jwtConfigOptions.Value;
    }

    /// <summary>
    /// Attempts to authenticate user by its <paramref name="login"/> 
    /// </summary>
    /// <param name="login">User's login string</param>
    /// <returns>Ok response with <see cref="AuthResponse"/> either <see cref="StatusCodes.Status401Unauthorized"/>
    /// if user with user with such login doesn't exist</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SignInAsync([FromBody] string login)
    {
        var user = await Context.Users.FirstOrDefaultAsync(user => user.Login == login);
        if (user is { })
        {
            _logger.LogInformation("Auth attempt succeeded with: login={Login}", login);
            var jwtToken = CreateJwtToken(user);
            return Ok(new AuthResponse(user.Id, jwtToken));
        }

        _logger.LogWarning("Auth attempt failed with: login={Login}", login);
        return Problem("User with such login doesn't exist", statusCode: StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Creates new JWT-token for user, that is valid next 12 hours
    /// </summary>
    /// <param name="user">Authorized user</param>
    /// <returns>String with JWT bearer token</returns>
    private string CreateJwtToken(User user)
    {
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            notBefore: now,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
            },
            expires: now.Add(TimeSpan.FromHours(12)),
            signingCredentials: new SigningCredentials(_jwtConfig.SecretKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}