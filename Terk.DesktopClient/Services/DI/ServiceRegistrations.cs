namespace Terk.DesktopClient.Services.DI;

public static class ServiceRegistrations
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        return services
            .AddSingleton(Json.DefaultSerializerOptions())
            .AddSingleton<HttpClient>()
            .AddSingleton<ApiRequester>();
    }
}