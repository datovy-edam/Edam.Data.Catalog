using Monaco;

namespace Edam.Tests.CatalogExplorer.Presentation;

public sealed partial class MainPage : Page
{
    private IMonacoEditorModel _editorModel = new MonacoModel();
    public MainPage()
    {
        this.InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        Editor.Tag = _editorModel;
        await Editor.InitializeEditorModelAsync();
        //await Editor.InitializeEditorModelAsync();
    }

}

public class MonacoModel : IMonacoEditorModel
{
    public string CurrentLanguage { get; set; } = "application/json";
    public string Content { get; set; } = "{ }";
}
