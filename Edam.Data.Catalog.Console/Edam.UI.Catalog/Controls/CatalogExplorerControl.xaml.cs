using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Edam.UI.CatalogExplorer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Edam.UI.Catalog.Controls;
public sealed partial class CatalogExplorerControl : UserControl
{
    private CatalogExplorerViewModel _ViewModel =
        new CatalogExplorerViewModel();
    public CatalogExplorerViewModel ViewModel
    {
        get { return _ViewModel; }
    }
    public CatalogExplorerControl()
    {
        this.InitializeComponent();
        DataContext = _ViewModel;
    }

    private async void UploadFile_Click(object sender, RoutedEventArgs e)
    {
        var file = await AppSession.GetFileAsync();
    }

    private async void UploadFolder_Click(object sender, RoutedEventArgs e)
    {
        var folder = await AppSession.GetFolderAsync();
    }

}
