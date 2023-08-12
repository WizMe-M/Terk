using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Terk.API.Abstractions;
using Terk.DB.Context;

namespace Terk.API.Controllers;

[Route("[controller]/[action]")]
public class AuthController : DbController
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, TerkDbContext context) : base(context)
    {
        _logger = logger;
    }

    /// <summary>
    /// Attempts user's signing in with 
    /// </summary>
    /// <param name="login">User's login string</param>
    /// <returns>Ok response with authenticated user either <see cref="StatusCodes.Status401Unauthorized"/>
    /// if user with user with such login doesn't exist</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SignInAsync([FromBody] string login)
    {
        var user = await Context.Users.FirstOrDefaultAsync(user => user.Login == login);
        if (user is { })
        {
            _logger.LogInformation("Auth attempt succeeded with: login={Login}", login);
            return Ok(user);
        }

        _logger.LogWarning("Auth attempt failed with: login={Login}", login);
        return Problem("User with such login doesn't exist", statusCode: StatusCodes.Status401Unauthorized);
    }
}