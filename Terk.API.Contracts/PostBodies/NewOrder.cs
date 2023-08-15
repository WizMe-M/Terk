namespace Terk.API.Contracts.PostBodies;

/// <summary>
/// New order created by user
/// </summary>
/// <param name="Positions">Positions of new order</param>
public record NewOrder(NewOrderPosition[] Positions);

/// <summary>
/// Position in new order
/// </summary>
/// <param name="Id">ID of product</param>
/// <param name="Count">Count of products</param>
public record NewOrderPosition(int Id, byte Count);