using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Edam.UI.Catalog;
using Edam.Data.CatalogModel;

namespace Edam.UI.CatalogExplorer;

public class CatalogViewModel : ObservableObject
{
    private string? _defaultConnectionString;
    private INavigator _navigator;

    private CatalogInfo? _Catalog = null;
    private ItemInfo? _Root = null;
    public ItemInfo? Root
    {
        get { return _Root; }
    }

    public CatalogViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
        _defaultConnectionString = 
            appInfo?.Value?.DefaultConnectionString;
    }

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async void InitializeCatalog()
    {
        if (_navigator != null)
        {
            var catalog = await
                CatalogServiceHelper.GetCatalogAsync(_defaultConnectionString);
            _Catalog = catalog;
            _Root = catalog.RootItem;
        }
    }

}
