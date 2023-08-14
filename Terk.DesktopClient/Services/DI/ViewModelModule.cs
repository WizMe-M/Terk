namespace Terk.DesktopClient.Services.DI;

public class ViewModelModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<MainWindowViewModel>(c => new MainWindowViewModel(c.Resolve<LoginViewModel>()))
            .AsSelf().SingleInstance();
        builder.RegisterType<LoginViewModel>().AsSelf().InstancePerDependency();
    }
}