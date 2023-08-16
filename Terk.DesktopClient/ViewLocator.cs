namespace Terk.DesktopClient;

/// <summary>
/// Matcher of ViewModels and Views
/// </summary>
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// Create View from ViewModel
    /// </summary>
    /// <param name="data">ViewModel</param>
    /// <returns>View that corresponds to ViewModel</returns>
    public IControl Build(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <summary>
    /// Checks whether object is ViewModel
    /// </summary>
    /// <param name="data">Some object to locate</param>
    /// <returns>Is ViewModel</returns>
    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}