namespace Terk.DesktopClient.ViewModels.Base;

/// <summary>
/// Represents ViewModel that holds and changes current content 
/// </summary>
public partial class MainContentViewModel : ViewModelBase
{
    /// <summary>
    /// Represents ViewModel that is displayed currently
    /// </summary>
    [ObservableProperty] private ContentViewModel _currentContentVm;
    
    /// <summary>
    /// Represents default content value
    /// </summary>
    private readonly ContentViewModel _defaultContentVm;

    public MainContentViewModel(ContentViewModel defaultContentVm)
    {
        _defaultContentVm = defaultContentVm;
        _currentContentVm = _defaultContentVm;
        _currentContentVm.ContentChanged += OnContentChanged;
    }

    /// <summary>
    /// Resets current content to default value
    /// </summary>
    public void ResetContent()
    {
        CurrentContentVm.ContentChanged -= OnContentChanged;
        CurrentContentVm = _defaultContentVm;
        CurrentContentVm.ContentChanged += OnContentChanged;
        CurrentContentVm.InitAsync();
    }

    /// <summary>
    /// Handles <see cref="ContentViewModel.ContentChanged"/> event
    /// </summary>
    /// <param name="sender">Event's sender</param>
    /// <param name="e">Args of changing content</param>
    /// <remarks>
    /// Changes current content.
    /// Handles subscription to <see cref="ContentViewModel.ContentChanged"/>.
    /// Initializes new content to display.
    /// </remarks>
    private void OnContentChanged(object? sender, MainContentChangedEventArgs e)
    {
        CurrentContentVm.ContentChanged -= OnContentChanged;
        CurrentContentVm = e.NewViewModel;
        CurrentContentVm.ContentChanged += OnContentChanged;
        CurrentContentVm.InitAsync();
    }
}