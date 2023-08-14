namespace Terk.DesktopClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private LoginViewModel _loginVm;
    [ObservableProperty] private ProfileViewModel _profileVm;
    [ObservableProperty] private ViewModelBase? _currentVm;

    [ObservableProperty] private bool _isSignedIn;

    public MainWindowViewModel(LoginViewModel loginVm, ProfileViewModel profileVm)
    {
        _loginVm = loginVm;
        _profileVm = profileVm;
        _loginVm.SignedIn += OnSignedIn;
        _profileVm.SignedOut += OnSignedOut;
    }

    private void OnSignedIn(object? sender, SignInEventArgs e) => IsSignedIn = true;
    private void OnSignedOut(object? sender, LogOutEventArgs e) => IsSignedIn = false;
}