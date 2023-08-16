namespace Terk.API.Contracts;

public static class Json
{
    /// <summary>
    /// Gets default setting for <see cref="JsonSerializerOptions"/> that are used on server and client
    /// </summary>
    /// <returns>Default settings</returns>
    public static JsonSerializerOptions DefaultSerializerOptions()
    {
        return new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }
}