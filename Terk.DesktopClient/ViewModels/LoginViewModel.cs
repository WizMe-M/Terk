namespace Terk.DesktopClient.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ApiRequester _apiRequester;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SignInCommand))]
    private string _login = "";

    public LoginViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
    }

    public bool CanSignIn => !string.IsNullOrWhiteSpace(Login);

    [RelayCommand(CanExecute = nameof(CanSignIn))]
    private async Task SignIn()
    {
        var successfulSignIn = await _apiRequester.SignIn(Login);
    }
}