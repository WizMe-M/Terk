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

    private async Task UploadOrders()
    {
        var orders = await _apiRequester.GetMyOrdersAsync();
        Orders.Clear();
        Orders.AddRange(orders);
    }

    [RelayCommand]
    private void OpenNewOrder() => ChangeContent(_placeNewOrderVm);
}