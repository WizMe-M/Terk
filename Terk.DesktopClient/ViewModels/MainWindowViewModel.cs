namespace Terk.DesktopClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private LoginViewModel _loginVm;
    [ObservableProperty] private ViewModelBase? _currentVm;

    public MainWindowViewModel(LoginViewModel loginVm)
    {
        _loginVm = loginVm;
        _loginVm.SignedIn += OnSignedIn;
    }

    private void OnSignedIn(object? sender, SignInEventArgs e)
    {
        Console.WriteLine("sign in");
    }
}