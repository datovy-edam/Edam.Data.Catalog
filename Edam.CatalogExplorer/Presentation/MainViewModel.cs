using Edam.UI.Catalog;

// -----------------------------------------------------------------------------
namespace Edam.CatalogExplorer.Presentation;

public partial class MainViewModel : ObservableObject
{
    private AppModelState _UiScope;
    public AppModelState UiScope
    {
        get { return _UiScope; }
    }

    private INavigator _navigator;

    [ObservableProperty]
    private string? name;

    public string? Title { get; }

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        // force pool initiation...
        Monaco.EditorPool.AvailablePools();

        _navigator = navigator;
        Title = "Catalog Explorer";

        _UiScope = new AppModelState()
        {
            Localizer = localizer,
            Navigator = navigator,
            AppOptions = appInfo
        };
    }

    //public ICommand GoToSecond { get; }

    //private async Task GoToSecondView()
    //{
    //    await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Name!));
    //}

}
