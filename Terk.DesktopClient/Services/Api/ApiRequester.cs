namespace Terk.DesktopClient.Services.Api;

public class ApiRequester : IAuthorizingClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public ApiRequester(HttpClient httpClient, JsonSerializerOptions options)
    {
        _options = options;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:4444/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
    }

    public void SetAuthorization(string token) => _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

    public void ResetAuthorization() => _httpClient.DefaultRequestHeaders.Authorization = null;

    public async Task<bool> SignInAsync(string login)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth", login, _options);
        if (!response.IsSuccessStatusCode) return false;
        
        var (_, jwtToken) = (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
        SetAuthorization(jwtToken);
        return true;
    }

    public async Task<Product[]> GetProducts()
    {
        var products = await _httpClient.GetFromJsonAsync<Product[]>("api/products", _options);
        return products!;
    }
    
    public async Task<Order[]> GetMyOrdersAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<Order[]>("api/orders/my", _options);
        return orders!;
    }

    public async Task<bool> CreateOrderAsync(NewOrder newOrder)
    {
        var response = await _httpClient.PostAsJsonAsync("api/orders/new", newOrder);
        return response.IsSuccessStatusCode;
    }
}