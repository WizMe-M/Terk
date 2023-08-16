namespace Terk.API.Abstractions;

/// <summary>
/// API-Controller with DB context
/// </summary>
[ApiController]
public class DbController : ControllerBase
{
    /// <summary>
    /// Context to access database
    /// </summary>
    protected readonly TerkDbContext Context;

    public DbController(TerkDbContext context)
    {
        Context = context;
    }
}