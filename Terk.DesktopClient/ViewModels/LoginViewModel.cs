namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents sidebar ViewModel that allows user to login
/// </summary>
public partial class LoginViewModel : SideBarViewModel
{
    /// <summary>
    /// Event that fires after user successfully signed in 
    /// </summary>
    public event EventHandler<SignInEventArgs>? SignedIn;

    /// <summary>
    /// Inputted user's login
    /// </summary>
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _login = "";

    private readonly IApiClient _apiClient;

    public LoginViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Can user call <see cref="SignInCommand"/>
    /// </summary>
    public bool CanSignIn => !string.IsNullOrWhiteSpace(Login);

    /// <summary>
    /// Request authentication to API
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {
        var signedIn = await _apiClient.SignInAsync(Login);
        if (signedIn)
        {
            Login = "";
            SignedIn?.Invoke(this, new SignInEventArgs());
        }
    }
}