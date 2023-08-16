namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents ViewModel that creates and saves new user's order
/// </summary>
public partial class PlaceNewOrderViewModel : ContentViewModel
{
    /// <summary>
    /// Current selected product in <see cref="Products"/> list
    /// </summary>
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddPositionCommand))]
    private Product? _selectedProduct;

    /// <summary>
    /// Inputted count of products to add in order
    /// </summary>
    [ObservableProperty] private int _count = 1;

    /// <summary>
    /// Total order's cost
    /// </summary>
    [ObservableProperty] private decimal _totalSum;

    /// <summary>
    /// Lazy value (to exclude cyclic dependency) of <see cref="MyOrdersViewModel"/>
    /// </summary>
    private readonly Lazy<MyOrdersViewModel> _myOrdersVm;

    private readonly IApiClient _apiClient;

    public PlaceNewOrderViewModel(IApiClient apiClient, Lazy<MyOrdersViewModel> myOrdersVm)
    {
        _apiClient = apiClient;
        _myOrdersVm = myOrdersVm;
        PositionsInOrder.CollectionChanged += OnPositionsListChanged;
    }

    /// <summary>
    /// List of all available products to add
    /// </summary>
    public ObservableCollection<Product> Products { get; } = new();

    /// <summary>
    /// Positions that already added to order
    /// </summary>
    public ObservableCollection<NewOrderPositionViewModel> PositionsInOrder { get; } = new();

    /// <summary>
    /// Can call <see cref="AddPositionCommand"/>
    /// </summary>
    public bool IsProductSelected => SelectedProduct is { };

    /// <summary>
    /// Can call <see cref="ProcessOrderCommand"/>
    /// </summary>
    public bool CanProcessOrder => PositionsInOrder.Count > 0;
    
    public override Task InitAsync() => UploadProducts();

    /// <summary>
    /// Add selected product to order or increment count of already added product
    /// </summary>
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

    /// <summary>
    /// Remove position from order
    /// </summary>
    /// <param name="position">Position to remove</param>
    [RelayCommand]
    private void RemovePosition(NewOrderPositionViewModel position)
    {
        PositionsInOrder.Remove(position);
    }

    /// <summary>
    /// Save and send info about new order to API
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanProcessOrder))]
    private async Task ProcessOrder()
    {
        var positions = PositionsInOrder
            .Select(position => new NewOrderPosition(position.Product.Id, (byte)position.Count))
            .ToArray();
        var newOrder = new NewOrder(positions);
        var isCreated = await _apiClient.CreateOrderAsync(newOrder);
        if (isCreated)
        {
            // TODO: maybe would be better to replace Lazy with some factory?
            ChangeContent(_myOrdersVm.Value);
        }
    }

    /// <summary>
    /// Handles change of <see cref="PositionsInOrder"/> list
    /// </summary>
    /// <param name="sender">List of positions</param>
    /// <param name="e">Args of collection changed event</param>
    private void OnPositionsListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ProcessOrderCommand.NotifyCanExecuteChanged();
        TotalSum = (decimal)PositionsInOrder.Select(position => position.TotalCost).Sum();
    }

    /// <summary>
    /// Refreshes list of products with data from API
    /// </summary>
    private async Task UploadProducts()
    {
        var orders = await _apiClient.GetProductsAsync();
        Products.Clear();
        Products.AddRange(orders);
    }
}