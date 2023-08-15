namespace Terk.DesktopClient.ViewModels;

public partial class PlaceNewOrderViewModel : ContentViewModel
{
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddPositionCommand))]
    private Product? _selectedProduct;

    [ObservableProperty] private int _count = 1;
    [ObservableProperty] private decimal _totalSum;
    private readonly ApiRequester _apiRequester;

    public PlaceNewOrderViewModel(ApiRequester apiRequester)
    {
        _apiRequester = apiRequester;
        PositionsInOrder.CollectionChanged += RecalculateTotalSum;
    }

    public ObservableCollection<Product> Products { get; } = new();

    public ObservableCollection<NewOrderPositionViewModel> PositionsInOrder { get; } = new();

    public bool IsProductSelected => SelectedProduct is { };

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

    private void RecalculateTotalSum(object? sender, NotifyCollectionChangedEventArgs e)
    {
        TotalSum = (decimal)PositionsInOrder.Select(position => position.TotalCost).Sum();
    }

    private async Task UploadProducts()
    {
        var orders = await _apiRequester.GetProducts();
        Products.Clear();
        Products.AddRange(orders);
    }
}