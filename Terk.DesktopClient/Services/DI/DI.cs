namespace Terk.DesktopClient.Services.DI;

public class DI
{
    public static IContainer Container => _container!;
    private static IContainer? _container;

    private readonly ContainerBuilder _builder = new();

    public DI Populate(IServiceCollection services)
    {
        _builder.Populate(services);
        return this;
    }

    public void Build() => _container ??= _builder.Build();
}