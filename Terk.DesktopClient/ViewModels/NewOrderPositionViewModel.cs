namespace Terk.DesktopClient.ViewModels;

public class NewOrderPositionViewModel : ViewModelBase
{
    public readonly Product Product;

    public NewOrderPositionViewModel(Product product, int count)
    {
        Product = product;
        Count = count;
    }
    public int Count { get; }

    public string Name => Product.Name;
    public double Cost => Product.Cost;
    public double TotalCost => Count * Cost;
}