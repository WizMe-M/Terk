namespace Terk.DesktopClient.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ApiRequester _apiRequester;

    [ObservableProperty] private string _login = "";

    public LoginViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
    }

    [RelayCommand]
    private async Task SignIn()
    {
        var successfulSignIn = await _apiRequester.SignIn(Login);
    }
}