using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Edam.Data.CatalogModel;
using Edam.Data.CatalogDb;
using Edam.UI.CatalogExplorer;
using System.Collections.ObjectModel;
using Edam.UI.Catalog.Models;
using Edam.Diagnostics;
using Edam.Application;

// -----------------------------------------------------------------------------

namespace Edam.UI.Catalog.Controls;

public class CatalogExplorerViewModel : ObservableObject
{

    public const string CATALOG_INITIALIZED = "CATALOG-INITIALIZED";

    private string? _defaultConnectionString;
    private INavigator _navigator;

    private CatalogInfo? _Catalog = null;
    private CatalogItem RootItem = null;

    public CatalogInfo? Catalog
    {
        get => _Catalog;
    }

    public ObservableCollection<CatalogItem> DataSource { get; set; } =
        new ObservableCollection<CatalogItem>();

    public NotificationEventHandler NotifyEvent { get; set; }

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async Task InitializeCatalogAsync(AppModelState state)
    {
        string connectionUri = state.GetConnectionUri();

        _Catalog = await CatalogServiceHelper.GetCatalogAsync(
            connectionUri);

        // get root element observable item
        DataSource.Clear();
        RootItem = GetData(_Catalog.RootTreeItem);

        // don't show root item so add first level items (the children)
        foreach(var itm in RootItem.Children)
        {
            DataSource.Add(itm);
        }

        if (NotifyEvent != null)
        {
            var args = new NotificationEventArgs
            {
                Results = new ResultLog(),
                EventID = CATALOG_INITIALIZED,
                Data = _Catalog
            };
            args.Results.Succeeded();
            NotifyEvent(this, args);
        }
    }

    /// <summary>
    /// Given a Catalog Item build corresponding observable item...
    /// </summary>
    /// <param name="item">item to go through children and build tree</param>
    /// <returns>observable item</returns>
    public CatalogItem GetData(CatalogItemInfo item)
    {
        CatalogItem itm = new CatalogItem()
        {
            Name = item.Name,
            Item = item,
        };

        foreach(var node in item.Children)
        {
            itm.Children.Add(GetData(node));
        }
        return itm;
    }
}

/// <summary>
/// Observable Item
/// </summary>
public class CatalogItem
{
    public string Name { get; set; }
    public ObservableCollection<CatalogItem> Children { get; set; } = 
        new ObservableCollection<CatalogItem>();
    public CatalogItemInfo Item { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
