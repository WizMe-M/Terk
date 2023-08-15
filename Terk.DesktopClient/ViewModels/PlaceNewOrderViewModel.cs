namespace Terk.DesktopClient.ViewModels;

public partial class PlaceNewOrderViewModel : ContentViewModel
{
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddPositionCommand))]
    private Product? _selectedProduct;

    [ObservableProperty] private int _count = 1;
    [ObservableProperty] private decimal _totalSum;
    private readonly ApiRequester _apiRequester;
    private readonly Lazy<MyOrdersViewModel> _myOrdersVm;

    public PlaceNewOrderViewModel(ApiRequester apiRequester, Lazy<MyOrdersViewModel> myOrdersVm)
    {
        _apiRequester = apiRequester;
        _myOrdersVm = myOrdersVm;
        PositionsInOrder.CollectionChanged += OnPositionsListChanged;
    }

    public ObservableCollection<Product> Products { get; } = new();

    public ObservableCollection<NewOrderPositionViewModel> PositionsInOrder { get; } = new();

    public bool IsProductSelected => SelectedProduct is { };
    
    public bool CanProcessOrder => PositionsInOrder.Count > 0;

    public override Task InitAsync() => UploadProducts();

    [RelayCommand(CanExecute = nameof(IsProductSelected))]
    private void AddPosition()
    {
        var position = PositionsInOrder.FirstOrDefault(position => position.Product == SelectedProduct);
        var count = Count;
        if (position is { })
        {
            PositionsInOrder.Remove(position);
            count += position.Count;
        }
        var newPosition = new NewOrderPositionViewModel(SelectedProduct!, count);
        PositionsInOrder.Add(newPosition);
        Count = 1;
    }

    [RelayCommand]
    private void RemovePosition(NewOrderPositionViewModel position)
    {
        PositionsInOrder.Remove(position);
    }

    [RelayCommand(CanExecute = nameof(CanProcessOrder))]
    private async Task ProcessOrder()
    {
        var positions = PositionsInOrder
            .Select(position => new NewOrderPosition(position.Product.Id, (byte)position.Count))
            .ToArray();
        var newOrder = new NewOrder(positions);
        var isCreated = await _apiRequester.CreateOrderAsync(newOrder);
        if (isCreated)
        {
            // TODO: maybe would be better to replace Lazy with some factory?
            ChangeContent(_myOrdersVm.Value);
        }
    }
    
    private void OnPositionsListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ProcessOrderCommand.NotifyCanExecuteChanged();
        TotalSum = (decimal)PositionsInOrder.Select(position => position.TotalCost).Sum();
    }

    private async Task UploadProducts()
    {
        var orders = await _apiRequester.GetProducts();
        Products.Clear();
        Products.AddRange(orders);
    }
}