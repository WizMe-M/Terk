namespace Terk.DesktopClient.ViewModels.Base;

/// <summary>
/// Template of ViewModel that represents some content in <see cref="MainContentViewModel"/>
/// </summary>
public class ContentViewModel : ViewModelBase
{
    /// <summary>
    /// Event that fires after current content has changed
    /// </summary>
    public event EventHandler<MainContentChangedEventArgs>? ContentChanged;

    /// <summary>
    /// Invokes <see cref="ContentChanged"/>
    /// </summary>
    /// <param name="newContentViewModel">New ViewModel</param>
    protected void ChangeContent(ContentViewModel newContentViewModel)
    {
        ContentChanged?.Invoke(this, new MainContentChangedEventArgs(newContentViewModel));
    }

    /// <summary>
    /// Initializes this ViewModel asynchronously
    /// </summary>
    public virtual Task InitAsync() => Task.CompletedTask;
}