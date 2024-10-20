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

    //private string? _defaultConnectionString;
    //private INavigator _navigator;

    public CatalogViewModel CatalogBase { get; set; }

    public ObservableCollection<CatalogItem> DataSource { get; set; } =
        new ObservableCollection<CatalogItem>();

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async Task InitializeCatalogAsync(AppModelState state)
    {
        await CatalogBase.GetCatalogAsync(state);

        // get root element observable item
        DataSource.Clear();
        CatalogBase.RootItem = GetData(CatalogBase.Catalog.RootTreeItem);

        // don't show root item so add first level items (the children)
        foreach(var itm in CatalogBase.RootItem.Children)
        {
            DataSource.Add(itm);
        }

        if (CatalogBase.NotifyEvent != null)
        {
            var args = new NotificationEventArgs
            {
                Results = new ResultLog(),
                EventID = CatalogViewModel.CATALOG_INITIALIZED,
                Data = CatalogBase.Catalog
            };
            args.Results.Succeeded();
            CatalogBase.NotifyEvent(this, args);
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
