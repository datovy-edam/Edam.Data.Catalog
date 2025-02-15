namespace Edam.CatalogExplorer.Presentation;

public sealed partial class MainPage : Page
{
    private MainViewModel? _ViewModel;
    public MainPage()
    {
        this.InitializeComponent();
    }

    private void Page_DataContextChanged(
        FrameworkElement sender, DataContextChangedEventArgs args)
    {
        if (DataContext != null)
        {
            _ViewModel = DataContext as MainViewModel;
            if (_ViewModel != null)
            {
                CatalogPanel.InitializeCatalog(_ViewModel.UiScope);
            }
        }
    }
}
