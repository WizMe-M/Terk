namespace Terk.DesktopClient.Events;

public class MainContentChangedEventArgs : EventArgs
{
    public MainContentChangedEventArgs(ContentViewModel newViewModel)
    {
        NewViewModel = newViewModel;
    }

    public ContentViewModel NewViewModel { get; }
}