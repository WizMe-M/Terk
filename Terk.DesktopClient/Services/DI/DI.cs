namespace Terk.DesktopClient.Services.DI;

public static class DI
{
    public static IContainer Container => _container!;
    private static IContainer? _container;

    public static void BuildContainer()
    {
        var builder = new ContainerBuilder();
        IncludeModules(builder);
        _container = builder.Build();
    }

    private static void IncludeModules(ContainerBuilder builder)
    {
        builder
            .RegisterModule<ApiModule>()
            .RegisterModule<ViewModelModule>();
    }
}