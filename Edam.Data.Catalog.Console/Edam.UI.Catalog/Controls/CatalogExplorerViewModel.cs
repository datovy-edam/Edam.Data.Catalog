using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Edam.Data.CatalogModel;
using Edam.Data.FileSystemDb;
using Edam.UI.CatalogExplorer;
using System.Collections.ObjectModel;

// -----------------------------------------------------------------------------

namespace Edam.UI.Catalog.Controls;

public class CatalogExplorerViewModel : ObservableObject
{

    private string? _defaultConnectionString;
    private INavigator _navigator;

    private CatalogInfo? _Catalog = null;
    private Item RootItem = null;

    public ObservableCollection<Item> DataSource { get; set; } =
        new ObservableCollection<Item>();

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async void InitializeCatalog(AppModelState state)
    {
        _Catalog = await CatalogServiceHelper.GetCatalogAsync(
            state.GetDefaultConnectionString());

        // get root element observable item
        DataSource.Clear();
        RootItem = GetData(_Catalog.RootItem);

        // don't show root item so add first level items (the children)
        foreach(var itm in RootItem.Children)
        {
            DataSource.Add(itm);
        }
    }

    /// <summary>
    /// Given a Catalog Item build corresponding observable item...
    /// </summary>
    /// <param name="item">item to go through children and build tree</param>
    /// <returns>observable item</returns>
    public Item GetData(CatalogItemInfo item)
    {
        Item itm = new Item()
        {
            Name = item.Name,
            CatalogItem = item,
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
public class Item
{
    public string Name { get; set; }
    public ObservableCollection<Item> Children { get; set; } = 
        new ObservableCollection<Item>();
    public CatalogItemInfo CatalogItem { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
