using Edam.UI.Catalog;

// -----------------------------------------------------------------------------
namespace Edam.CatalogExplorer.Presentation;

public partial class MainViewModel : ObservableObject
{
    private static AppModelState _AppState;
    public static AppModelState ApplicationState
    {
        get { return _AppState; }
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

        _AppState = new AppModelState()
        {
            Localizer = localizer,
            Navigator = navigator,
            AppOptions = appInfo
        };

        AppSession.ApplicationState = _AppState;
    }

}
