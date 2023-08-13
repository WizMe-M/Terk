namespace Terk.API.Abstractions;

[ApiController]
public class DbController : ControllerBase
{
    protected readonly TerkDbContext Context;

    public DbController(TerkDbContext context)
    {
        Context = context;
    }
}