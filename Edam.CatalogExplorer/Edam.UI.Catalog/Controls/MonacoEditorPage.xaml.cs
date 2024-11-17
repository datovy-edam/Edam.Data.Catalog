using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Edam.Data.CatalogModel;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Edam.UI.Catalog.Controls;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MonacoEditorPage : Page
{
    private MonacoEditorViewModel _ViewModel = new MonacoEditorViewModel();
    public MonacoEditorViewModel ViewModel
    {
        get { return _ViewModel; }
    }

    public MonacoEditorPage()
    {
        this.InitializeComponent();
        this.DataContext = ViewModel;
        Editor.SetEditorMiniMapVisible(false);
        Editor.MonacoEditorLoaded += EditorLoaded;
    }

    public void EditorLoaded(object sender, EventArgs e)
    {
        Editor.SetLanguageAsync(_ViewModel.CurrentLanguage);
        Editor.LoadContentAsync(_ViewModel.Content);
    }

    public async Task<string> GetContent()
    {
        return await Editor.GetEditorContentAsync();
    }

    public void SetEditor(string language, string content)
    {
        _ViewModel.CurrentLanguage = language;
        _ViewModel.Content = content;
    }

    public void SetPathItem(CatalogPathItem item)
    {
        ViewModel.CurrentPathItem = item;
    }
}
