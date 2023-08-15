namespace Terk.DesktopClient.ViewModels;

public partial class LoginViewModel : SideBarViewModel
{
    public event EventHandler<SignInEventArgs>? SignedIn;

    private readonly ApiRequester _apiRequester;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _login = "";

    [ObservableProperty] private bool _isSignedIn;

    public LoginViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
    }

    public bool CanSignIn => !string.IsNullOrWhiteSpace(Login);

    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {
        var signedIn = await _apiRequester.SignInAsync(Login);
        if (signedIn)
        {
            Login = "";
            SignedIn?.Invoke(this, new SignInEventArgs());
        }
    }
}