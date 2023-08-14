namespace Terk.API.Contracts;

public static class Json
{
    public static JsonSerializerOptions DefaultSerializerOptions()
    {
        return new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }
}