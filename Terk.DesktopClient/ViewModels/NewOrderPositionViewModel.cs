namespace Terk.DesktopClient.ViewModels;

/// <summary>
/// Represents position that will be added to new order
/// </summary>
public class NewOrderPositionViewModel : ViewModelBase
{
    /// <summary>
    /// Product in order
    /// </summary>
    public readonly Product Product;

    public NewOrderPositionViewModel(Product product, int count)
    {
        Product = product;
        Count = count;
    }

    /// <summary>
    /// Count of products
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Product's name
    /// </summary>
    public string Name => Product.Name;

    /// <summary>
    /// Product's cost
    /// </summary>
    public double Cost => Product.Cost;

    /// <summary>
    /// Total position's cost
    /// </summary>
    public double TotalCost => Count * Cost;
}