namespace Terk.DesktopClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentViewModel;

    public MainWindowViewModel(ViewModelBase loginViewModel)
    {
        _currentViewModel = loginViewModel;
    }
}