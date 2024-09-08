using Edam.Application;
using Edam.UI.Catalog;
using Edam.UI.Catalog.Controls;
namespace Edam.Data.Catalog.Console.Presentation;

// -----------------------------------------------------------------------------

public partial class MainViewModel : ObservableObject
{
    private AppModelState _UiScope;
    public AppModelState UiScope
    {
        get { return _UiScope; }
    }

    public string? Title { get; }
    public ICommand GoToSecond { get; }

    [ObservableProperty]
    private string? name;

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appOptions,
        INavigator navigator)
    {
        Title = "Sample App";

        _UiScope = new AppModelState()
        {
            Localizer = localizer,
            Navigator = navigator,
            AppOptions = appOptions
        };
    }



}
