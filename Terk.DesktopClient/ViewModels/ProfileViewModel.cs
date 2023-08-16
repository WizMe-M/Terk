namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents ViewModel that allows user to sign out
/// </summary>
public partial class ProfileViewModel : SideBarViewModel
{
    /// <summary>
    /// Event that fires after user signed out
    /// </summary>
    public event EventHandler<LogOutEventArgs>? SignedOut;

    private readonly IApiClient _apiClient;

    public ProfileViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Sign out user from system and <see cref="IApiClient"/>
    /// </summary>
    [RelayCommand]
    private void SignOut()
    {
        _apiClient.ResetAuthorization();
        SignedOut?.Invoke(this, new LogOutEventArgs());
    }
}