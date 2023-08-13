namespace Terk.API.Responses;

/// <summary>
/// User's order
/// </summary>
/// <param name="Id">Order's id</param>
/// <param name="CreatedDate">Date and time when order was crated</param>
/// <param name="Positions">Positions in order</param>
/// <param name="TotalCost">Total cost of order</param>
public record Order(int Id, DateTime CreatedDate, Position[] Positions, decimal TotalCost);

/// <summary>
/// Order's position
/// </summary>
/// <param name="Id">Position's id</param>
/// <param name="Product">Product in position</param>
/// <param name="Count">Count of products</param>
/// <param name="Cost">Position's cost
/// (<see cref="Product"/>.<see cref="Responses.Product.Cost"/> * <see cref="Count"/>)</param>
public record Position(int Id, Product Product, byte Count, decimal Cost);

/// <summary>
/// Product in order
/// </summary>
/// <param name="Id">Product's id</param>
/// <param name="Name">Product's name</param>
/// <param name="Cost">Product's cost</param>
public record Product(int Id, string Name, double Cost);