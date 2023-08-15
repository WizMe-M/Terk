namespace Terk.DesktopClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private SideBarViewModel _sideBarVm;
    [ObservableProperty] private MainContentViewModel? _mainContentVm;
    [ObservableProperty] private bool _isSignedIn;

    private readonly LoginViewModel _loginVm;
    private readonly ProfileViewModel _profileVm;
    private readonly MyOrdersViewModel _myOrdersVm;

    public MainWindowViewModel(LoginViewModel loginVm, ProfileViewModel profileVm, MyOrdersViewModel myOrdersVm)
    {
        _loginVm = loginVm;
        _profileVm = profileVm;
        _myOrdersVm = myOrdersVm;

        _sideBarVm = _loginVm;
        _loginVm.SignedIn += OnSignedIn;
        _profileVm.SignedOut += OnSignedOut;
    }

    private void OnSignedIn(object? sender, SignInEventArgs e)
    {
        IsSignedIn = true;
        SideBarVm = _profileVm;
        MainContentVm = _myOrdersVm;
        Task.Run(_myOrdersVm.UploadOrders);
    }

    private void OnSignedOut(object? sender, LogOutEventArgs e)
    {
        IsSignedIn = false;
        SideBarVm = _loginVm;
        MainContentVm = null;
    }
}