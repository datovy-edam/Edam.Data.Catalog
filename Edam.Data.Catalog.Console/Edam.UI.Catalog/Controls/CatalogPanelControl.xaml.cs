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
    private CatalogViewModel _ViewModel = new CatalogViewModel();

    public CatalogPanelControl()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Initialize Catalog.
    /// </summary>
    private async void InitializeCatalogAsync()
    {
        if (_ViewModel.State != null && IsLoaded && !_ViewModel.HasCatalog)
        {
            _ViewModel.HasCatalog = true;

            CatalogExplorer.ViewModel.CatalogBase = _ViewModel;
            CatalogContainer.ViewModel.CatalogBase = _ViewModel;

            await CatalogExplorer.ViewModel.
                InitializeCatalogAsync(_ViewModel.State);
            await CatalogContainer.ViewModel.InitializeContainersAsync();
        }
    }

    /// <summary>
    /// Initialize Catalog with given state.
    /// </summary>
    /// <param name="state"></param>
    public void InitializeCatalog(AppModelState state)
    {
        _ViewModel.State = state;
        InitializeCatalogAsync();
    }

    /// <summary>
    /// After form is loaded try initializing the Catalog.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeCatalogAsync();
    }

}
