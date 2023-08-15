namespace Terk.DesktopClient.ViewModels.Base;

public class ContentViewModel : ViewModelBase
{
    public event EventHandler<MainContentChangedEventArgs>? ContentChanged;

    protected void ChangeContent(ContentViewModel newContentViewModel)
    {
        ContentChanged?.Invoke(this, new MainContentChangedEventArgs(newContentViewModel));
    }

    public virtual Task InitAsync() => Task.CompletedTask;
}