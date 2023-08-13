namespace Terk.DesktopClient.Services.Api;

public interface IAuthorizingClient
{
    void SetAuthorization(string token);
    void ResetAuthorization();
}