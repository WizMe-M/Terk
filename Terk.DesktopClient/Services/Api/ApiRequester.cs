namespace Terk.DesktopClient.Services.Api;

public class ApiRequester : IAuthorizingClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public ApiRequester(HttpClient httpClient, JsonSerializerOptions options)
    {
        _options = options;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:4444/api/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
    }

    public void SetAuthorization(string token) => _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

    public void ResetAuthorization() => _httpClient.DefaultRequestHeaders.Authorization = null;
}