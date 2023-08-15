namespace Terk.DesktopClient.Services.DI;

public class ViewModelModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<LoginViewModel>().AsSelf().InstancePerDependency();
        builder.RegisterType<ProfileViewModel>().AsSelf().InstancePerDependency();
        builder.Register<MainContentViewModel>(c => new MainContentViewModel(c.Resolve<MyOrdersViewModel>()))
            .AsSelf().InstancePerDependency();
        builder.RegisterType<MyOrdersViewModel>().AsSelf().InstancePerDependency();
        builder.RegisterType<PlaceNewOrderViewModel>().AsSelf().InstancePerDependency();
    }
}