namespace Terk.DesktopClient.Events;

/// <summary>
/// Args for event of changing "Main content"
/// </summary>
public class MainContentChangedEventArgs : EventArgs
{
    public MainContentChangedEventArgs(ContentViewModel newViewModel)
    {
        NewViewModel = newViewModel;
    }

    /// <summary>
    /// New ViewModel that replaced previous ViewModel
    /// </summary>
    public ContentViewModel NewViewModel { get; }
}