namespace Terk.DesktopClient.ViewModels;

public partial class MyOrdersViewModel : ContentViewModel
{
    private readonly ApiRequester _apiRequester;
    private readonly PlaceNewOrderViewModel _placeNewOrderVm;

    public MyOrdersViewModel(ApiRequester apiRequester, PlaceNewOrderViewModel placeNewOrderVm)
    {
        _apiRequester = apiRequester;
        _placeNewOrderVm = placeNewOrderVm;
    }

    public ObservableCollection<Order> Orders { get; } = new();

    public override Task InitAsync() => UploadOrders();

    [RelayCommand]
    private void OpenNewOrder() => ChangeContent(_placeNewOrderVm);

    [RelayCommand]
    private async Task SaveFileWithOrders()
    {
        var response = await _apiRequester.DownloadFileWithMyOrders();
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
            var window = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow;
            var saveFilePath = await dialog.ShowAsync(window);
            if (saveFilePath is { })
            {
                await File.WriteAllBytesAsync(saveFilePath, response.Data);
            }
        }
    }

    private async Task UploadOrders()
    {
        var orders = await _apiRequester.GetMyOrdersAsync();
        Orders.Clear();
        Orders.AddRange(orders);
    }
}