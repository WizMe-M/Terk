namespace Terk.DesktopClient.Services.DI;

/// <summary>
/// Module that registers services to send requests to API
/// </summary>
public class ApiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Json.DefaultSerializerOptions()).AsSelf().SingleInstance();
        builder.RegisterType<HttpClient>().AsSelf().InstancePerDependency();
        builder.RegisterType<ApiRequester>().As<IApiClient>().SingleInstance();
    }
}