using Order = Terk.API.Contracts.Responses.Order;
using Position = Terk.API.Contracts.Responses.Position;
using Product = Terk.API.Contracts.Responses.Product;
using IoFile = System.IO.File;

namespace Terk.API.Controllers;

/// <summary>
/// Controller that allows to interact with orders
/// </summary>
[Route("api/orders")]
public class OrderController : DbController
{
    private readonly ILogger<OrderController> _logger;

    /// <summary>
    /// Info about environment (is needed to get <see cref="IWebHostEnvironment.WebRootPath"/>)
    /// </summary>
    private readonly IWebHostEnvironment _environment;

    public OrderController(TerkDbContext context, ILogger<OrderController> logger, IWebHostEnvironment environment) :
        base(context)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Receives user's orders
    /// </summary>
    /// <param name="authorization">User's JWT bearer authorization token</param>
    /// <returns>Ok with array of <see cref="Order"/> either <see cref="ProblemDetails"/> with error's message</returns>
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
    /// Generates and sends info about orders as text file to user
    /// </summary>
    /// <param name="authorization">User's JWT bearer authorization token</param>
    /// <returns>Ok with attachment (.txt file) either <see cref="ProblemDetails"/> with error message</returns>
    [HttpGet("my/file")]
    public async Task<IActionResult> DownloadMyOrders([FromHeader] string authorization)
    {
        #region auth user and get his orders

        var id = authorization.ExtractId();
        if (id is null)
        {
            _logger.LogWarning("Unable to extract id from JWT: {Jwt}", authorization);
            return Problem("Incorrect authorization token", statusCode: StatusCodes.Status400BadRequest);
        }

        var user = Context.Users
            .Include(u => u.Orders)
            .ThenInclude(order => order.OrderPositions)
            .ThenInclude(position => position.Product)
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            _logger.LogWarning("User with ID in jwi: {Id}", id);
            return Problem("User not found", statusCode: StatusCodes.Status400BadRequest);
        }

        #endregion

        #region generate files' names and paths

        const string fileFolder = "files";
        var filesRootPath = Path.Join(_environment.WebRootPath, fileFolder);
        if (!Path.Exists(filesRootPath)) Directory.CreateDirectory(filesRootPath);

        var serverFileName = $"{Guid.NewGuid().ToString()}.txt";
        var serverFilePath = Path.Join(filesRootPath, serverFileName);
        var clientFileName = $"{user.Login} {DateTime.Now}.txt";

        #endregion

        #region generate text to write in files

        var orders = user.Orders.OrderByDescending(order => order.CreatedDate).ToArray();

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Заказы пользователя \"{user.Name}\":");
        foreach (var order in orders)
        {
            stringBuilder.AppendLine($"Заказ от {order.CreatedDate} на {order.TotalCost} руб.");
            foreach (var position in order.OrderPositions)
            {
                stringBuilder.AppendLine(
                    $"- {position.ProductCount} шт. {position.Product.Name} ({position.Product.Cost} руб.) - Итого: {position.Cost} руб."
                );
            }

            stringBuilder.AppendLine();
        }

        #endregion

        var fileContent = stringBuilder.ToString();
        await IoFile.WriteAllTextAsync(serverFilePath, fileContent);

        _logger.LogInformation("""
                               Requested user's orders file;
                               File on server ({ServerFile}) for user ({UserLogin});
                               Count of orders: {OrderCount};
                               File name for client: "{ClientFile}".
                               """,
            serverFileName, user.Login, orders.Length, clientFileName
        );

        return PhysicalFile(serverFilePath, MediaTypeNames.Text.Plain, clientFileName);
    }

    /// <summary>
    /// Creates new order for user
    /// </summary>
    /// <param name="authorization">User's JWT bearer authorization token</param>
    /// <param name="newOrder">Data for new order</param>
    /// <returns>Ok either <see cref="ProblemDetails"/> with error message</returns>
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