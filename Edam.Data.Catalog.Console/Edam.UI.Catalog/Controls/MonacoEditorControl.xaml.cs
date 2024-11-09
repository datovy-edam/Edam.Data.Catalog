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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Edam.UI.Catalog.Controls;

public sealed partial class MonacoEditorControl : UserControl
{
    private MonacoEditorViewModel _ViewModel = new MonacoEditorViewModel();
    public MonacoEditorViewModel ViewModel
    {
        get { return _ViewModel; }
    }

    public MonacoEditorControl()
    {
        this.InitializeComponent();
        Editor.SetEditorMiniMapVisible(false);
    }

    public async Task<string> GetContent()
    {
        return await Editor.GetEditorContentAsync();
    }

    public async Task SetEditor(string language, string content)
    {
        _ViewModel.CurrentLanguage = language;
        await Editor.SetLanguageAsync(language);
        await Editor.LoadContentAsync(content);
    }
}
