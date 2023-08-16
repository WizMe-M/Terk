namespace Terk.DesktopClient.Services.Api;

/// <summary>
/// Wrap for HTTP client to interact with API
/// </summary>
public interface IApiClient
{
    /// <summary>
    /// Adds authorization header to API requests
    /// </summary>
    /// <param name="token">JWT bearer token</param>
    void SetAuthorization(string token);

    /// <summary>
    /// Removes authorization header from API requests
    /// </summary>
    void ResetAuthorization();

    /// <summary>
    /// Authenticates user by login
    /// </summary>
    /// <param name="login">User's login</param>
    /// <returns>Was user successfully authenticated</returns>
    Task<bool> SignInAsync(string login);

    /// <summary>
    /// Gets list of products
    /// </summary>
    /// <returns>Array of <see cref="Product"/></returns>
    Task<Product[]> GetProductsAsync();

    /// <summary>
    /// Gets user's orders
    /// </summary>
    /// <returns>Array of user's <see cref="Order"/></returns>
    Task<Order[]> GetMyOrdersAsync();

    /// <summary>
    /// Downloads text (.txt) file with user's orders
    /// </summary>
    /// <returns><see cref="FileResponse"/>, either null if response wasn't successful</returns>
    Task<FileResponse?> DownloadMyOrdersAsync();

    /// <summary>
    /// Creates new order for user
    /// </summary>
    /// <param name="newOrder">Data to create order</param>
    /// <returns>Is order was created successfully</returns>
    Task<bool> CreateOrderAsync(NewOrder newOrder);
}