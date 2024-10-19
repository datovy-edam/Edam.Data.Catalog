using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Edam.UI.Catalog.Controls;
public sealed partial class CatalogPanelControl : UserControl
{

    private AppModelState _state;
    private bool _hasCatalog = false;

    public CatalogPanelControl()
    {
        this.InitializeComponent();
    }

    public async void InitializeCatalog(AppModelState state)
    {
        this._state = state;
        if (IsLoaded && !_hasCatalog)
        {
            _hasCatalog = true;
            //await CatalogExplorer.ViewModel.InitializeCatalogAsync(_state);
        }
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_state == null || _hasCatalog)
        {
            return;
        }
        _hasCatalog = true;
        //await CatalogExplorer.ViewModel.InitializeCatalogAsync(_state);
    }
}
