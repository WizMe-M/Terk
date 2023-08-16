namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents root ViewModel
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Current sidebar ViewModel
    /// </summary>
    [ObservableProperty] private SideBarViewModel _sideBarVm;

    /// <summary>
    /// Main content ViewModel
    /// </summary>
    [ObservableProperty] private MainContentViewModel _mainContentVm;

    /// <summary>
    /// Current state of user's authentication
    /// </summary>
    [ObservableProperty] private bool _isSignedIn;

    /// <summary>
    /// Login ViewModel
    /// </summary>
    private readonly LoginViewModel _loginVm;

    /// <summary>
    /// ProfileViewModel
    /// </summary>
    private readonly ProfileViewModel _profileVm;

    public MainWindowViewModel(LoginViewModel loginVm, ProfileViewModel profileVm, MainContentViewModel mainContentVm)
    {
        _loginVm = loginVm;
        _profileVm = profileVm;
        _mainContentVm = mainContentVm;

        _sideBarVm = _loginVm;
        _loginVm.SignedIn += OnSignedIn;
        _profileVm.SignedOut += OnSignedOut;
    }

    /// <summary>
    /// Handles <see cref="LoginViewModel.SignedIn"/> event
    /// </summary>
    /// <param name="sender">Event's sender</param>
    /// <param name="e">Args</param>
    private void OnSignedIn(object? sender, SignInEventArgs e)
    {
        IsSignedIn = true;
        SideBarVm = _profileVm;
        MainContentVm.CurrentContentVm.InitAsync();
    }

    /// <summary>
    /// Handles <see cref="ProfileViewModel.SignedOut"/>
    /// </summary>
    /// <param name="sender">Event's sender</param>
    /// <param name="e">Args</param>
    private void OnSignedOut(object? sender, LogOutEventArgs e)
    {
        IsSignedIn = false;
        SideBarVm = _loginVm;
        MainContentVm.ResetContent();
    }
}