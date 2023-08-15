namespace Terk.DesktopClient.ViewModels.Base;

public partial class MainContentViewModel : ViewModelBase
{
    [ObservableProperty] private ContentViewModel _currentContentVm;
    private readonly ContentViewModel _defaultContentVm;

    public MainContentViewModel(ContentViewModel defaultContentVm)
    {
        _defaultContentVm = defaultContentVm;
        _currentContentVm = _defaultContentVm;
        _currentContentVm.ContentChanged += OnContentChanged;
    }

    public void ResetContent()
    {
        CurrentContentVm.ContentChanged -= OnContentChanged;
        CurrentContentVm = _defaultContentVm;
        CurrentContentVm.ContentChanged += OnContentChanged;
        CurrentContentVm.InitAsync();
    }

    private void OnContentChanged(object? sender, MainContentChangedEventArgs e)
    {
        CurrentContentVm.ContentChanged -= OnContentChanged;
        CurrentContentVm = e.NewViewModel;
        CurrentContentVm.ContentChanged += OnContentChanged;
        CurrentContentVm.InitAsync();
    }
}