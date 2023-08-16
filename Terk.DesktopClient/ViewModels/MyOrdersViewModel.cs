namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents ViewModel with authorized user's orders
/// </summary>
public partial class MyOrdersViewModel : ContentViewModel
{
    private readonly IApiClient _apiClient;
    private readonly PlaceNewOrderViewModel _placeNewOrderVm;

    public MyOrdersViewModel(IApiClient apiClient, PlaceNewOrderViewModel placeNewOrderVm)
    {
        _apiClient = apiClient;
        _placeNewOrderVm = placeNewOrderVm;
    }

    /// <summary>
    /// User's orders
    /// </summary>
    public ObservableCollection<Order> Orders { get; } = new();

    public override Task InitAsync() => UploadOrders();

    /// <summary>
    /// Opens <see cref="PlaceNewOrderViewModel"/>
    /// </summary>
    [RelayCommand]
    private void OpenNewOrder() => ChangeContent(_placeNewOrderVm);

    /// <summary>
    /// Downloads user's orders as text file
    /// </summary>
    [RelayCommand]
    private async Task SaveFileWithOrders()
    {
        var response = await _apiClient.DownloadMyOrdersAsync();
        if (response is { })
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить файл с заказами",
                InitialFileName = response.Name,
                Filters = new List<FileDialogFilter>
                {
                    new() { Name = "Text file", Extensions = new List<string> { "txt" } }
                },
                DefaultExtension = ".txt"
            };
            var window = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!)
                .MainWindow;
            var saveFilePath = await dialog.ShowAsync(window);
            if (saveFilePath is { })
            {
                await File.WriteAllBytesAsync(saveFilePath, response.Data);
            }
        }
    }

    /// <summary>
    /// Refreshes user's orders with data from API
    /// </summary>
    private async Task UploadOrders()
    {
        var orders = await _apiClient.GetMyOrdersAsync();
        Orders.Clear();
        Orders.AddRange(orders);
    }
}