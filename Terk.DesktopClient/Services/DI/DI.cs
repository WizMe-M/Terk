namespace Terk.DesktopClient.Services.DI;

/// <summary>
/// Entry point to interact with DI
/// </summary>
public static class DI
{
    /// <summary>
    /// DI container that can resolve services
    /// </summary>
    public static IContainer Container => _container!;
    private static IContainer? _container;

    /// <summary>
    /// Creates and builds container
    /// </summary>
    public static void BuildContainer()
    {
        var builder = new ContainerBuilder();
        IncludeModules(builder);
        _container = builder.Build();
    }

    /// <summary>
    /// Registers necessary <see cref="Module"/>s to container
    /// </summary>
    /// <param name="builder">Container builder in which to register modules</param>
    private static void IncludeModules(ContainerBuilder builder)
    {
        builder
            .RegisterModule<ApiModule>()
            .RegisterModule<ViewModelModule>();
    }
}