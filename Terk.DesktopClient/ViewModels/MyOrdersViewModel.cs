namespace Terk.DesktopClient.ViewModels;

public class MyOrdersViewModel : MainContentViewModel
{
    private readonly ApiRequester _apiRequester;

    public MyOrdersViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
    }

    public ObservableCollection<Order> Orders { get; } = new();

    public async Task UploadOrders()
    {
        var orders = await _apiRequester.GetMyOrdersAsync();
        Orders.Clear();
        Orders.AddRange(orders);
    }
    // TODO: переход на страницу с оформлением нового заказа
}