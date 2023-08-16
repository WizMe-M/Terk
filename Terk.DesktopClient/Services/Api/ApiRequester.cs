namespace Terk.DesktopClient.Services.Api;

/// <summary>
/// API HTTP client 
/// </summary>
public class ApiRequester : IApiClient
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

    public async Task<Product[]> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<Product[]>("api/products", _options);
        return products!;
    }

    public async Task<Order[]> GetMyOrdersAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<Order[]>("api/orders/my", _options);
        return orders!;
    }

    public async Task<FileResponse?> DownloadMyOrdersAsync()
    {
        var response = await _httpClient.GetAsync("api/orders/my/file");
        if (!response.IsSuccessStatusCode ||
            !response.Content.Headers.TryGetValues("Content-Disposition", out var values)) return null;
        
        var disposition = ContentDispositionHeaderValue.Parse(values.First());
        var fileName = disposition.FileNameStar!;
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return new FileResponse(fileName, bytes);
    }

    public async Task<bool> CreateOrderAsync(NewOrder newOrder)
    {
        var response = await _httpClient.PostAsJsonAsync("api/orders/new", newOrder);
        return response.IsSuccessStatusCode;
    }
}