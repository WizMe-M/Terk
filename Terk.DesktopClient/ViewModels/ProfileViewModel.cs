namespace Terk.DesktopClient.ViewModels;

public partial class ProfileViewModel : SideBarViewModel
{
    public event EventHandler<LogOutEventArgs>? SignedOut;
    private readonly ApiRequester _apiRequester;

    public ProfileViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
    }

    [RelayCommand]
    private void SignOut()
    {
        _apiRequester.ResetAuthorization();
        SignedOut?.Invoke(this, new LogOutEventArgs());
    }
}