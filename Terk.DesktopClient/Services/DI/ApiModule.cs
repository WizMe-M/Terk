namespace Terk.DesktopClient.Services.DI;

public class ApiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Json.DefaultSerializerOptions()).AsSelf().SingleInstance();
        builder.RegisterType<HttpClient>().AsSelf().InstancePerDependency();
        builder.RegisterType<ApiRequester>().AsSelf().SingleInstance();
    }
}