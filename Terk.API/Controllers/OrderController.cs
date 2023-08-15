using Terk.API.Contracts.PostBodies;
using Order = Terk.API.Contracts.Responses.Order;
using Position = Terk.API.Contracts.Responses.Position;
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
    /// <returns>Ok with array of <see cref="Order"/> either <see cref="ProblemDetails"/> with message</returns>
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
            .OrderByDescending(order => order.CreatedDate)
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
                order.TotalCost))
            .ToArrayAsync();
        _logger.LogInformation("User (id={Id}) requested his orders (count={Count})", id, orders.Length);
        return Ok(orders);
    }

    /// <summary>
    /// Creates new order for user
    /// </summary>
    /// <param name="authorization">User's authorization token</param>
    /// <param name="newOrder">Data for new order</param>
    /// <returns>Ok either <see cref="ProblemDetails"/></returns>
    [HttpPost("new")]
    public async Task<IActionResult> CreateNewOrder([FromHeader] string authorization, NewOrder newOrder)
    {
        var id = authorization.ExtractId();
        if (id is null)
        {
            _logger.LogWarning("Unable to extract id from JWT: {Jwt}", authorization);
            return Problem("Incorrect authorization token", statusCode: StatusCodes.Status400BadRequest);
        }

        // select products by ID
        var productCosts =
            Context.Products.Where(product => newOrder.Positions
                .Select(nop => nop.Id)
                .Contains(product.Id)
            ).ToArray();

        // create order's positions
        var positions = newOrder.Positions.Select(nop => new OrderPosition
        {
            ProductId = nop.Id,
            ProductCount = nop.Count,
            Cost = (decimal)productCosts.First(productCost => productCost.Id == nop.Id).Cost * nop.Count,
        }).ToArray();

        // create order for user
        var order = new Terk.DB.Entities.Order
        {
            CustomerId = id.Value,
            OrderPositions = positions,
            TotalCost = positions.Select(position => position.Cost).Sum(),
        };

        Context.Orders.Add(order);
        await Context.SaveChangesAsync();

        return Ok("Order successfully created");
    }
}