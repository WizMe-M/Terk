using Order = Terk.API.Contracts.Responses.Order;
using Product = Terk.API.Contracts.Responses.Product;

namespace Terk.API.Controllers;

[Route("api/orders")]
public class OrderController : DbController
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(TerkDbContext context, ILogger<OrderController> logger) : base(context)
    {
        _logger = logger;
    }

    /// <summary>
    /// Requests user's orders
    /// </summary>
    /// <param name="authorization">JWT Bearer authorization token</param>
    /// <returns>Ok with array of <see cref="Order"/> either <see cref="ProblemDetails"/>
    /// if user can't be authorized through jwt token</returns>
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders([FromHeader] string authorization)
    {
        var id = authorization.ExtractId();
        if (id is null)
        {
            _logger.LogWarning("Unable to extract id from JWT: {Jwt}", authorization);
            return Problem("Incorrect authorization token", statusCode: StatusCodes.Status400BadRequest);
        }

        var orders = await Context.Orders
            .Include(order => order.OrderPositions)
            .Where(order => order.CustomerId == id)
            .Select(order => new Order(
                order.Id,
                order.CreatedDate,
                order.OrderPositions.Select(position => new Position(
                    position.Id,
                    new Product(
                        position.Product.Id,
                        position.Product.Name,
                        position.Product.Cost),
                    position.ProductCount,
                    position.Cost)
                ).ToArray(),
                order.TotalCost)
            ).ToArrayAsync();
        _logger.LogInformation("User (id={Id}) requested his orders (count={Count})", id, orders.Length);
        return Ok(orders);
    }
}