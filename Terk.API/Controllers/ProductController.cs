﻿using Product = Terk.API.Contracts.Responses.Product;

namespace Terk.API.Controllers;

/// <summary>
/// Controller that allows to interact with products
/// </summary>
[Route("api/products")]
public class ProductController : DbController
{
    private readonly ILogger<ProductController> _logger;

    public ProductController(TerkDbContext context, ILogger<ProductController> logger) : base(context)
    {
        _logger = logger;
    }

    /// <summary>
    /// Receive all available products
    /// </summary>
    /// <param name="authorization">User's JWT bearer authorization token</param>
    /// <returns>Ok with array of <see cref="Product"/> either <see cref="ProblemDetails"/> with error message</returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromHeader] string authorization)
    {
        var id = authorization.ExtractId();
        if (id is { })
        {
            var user = await Context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user is { })
            {
                var products = await Context.Products
                    .Select(product => new Product(product.Id, product.Name, product.Cost))
                    .ToArrayAsync();
                return Ok(products);
            }

            _logger.LogWarning("Incorrect user's id={Id}", id);
        }

        _logger.LogWarning("Incorrect jwt token: {Jwt}", authorization);
        return Problem($"Authorization token is incorrect (token={authorization}",
            statusCode: StatusCodes.Status401Unauthorized);
    }
}