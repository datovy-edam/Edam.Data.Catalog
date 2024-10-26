using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Edam.Data.CatalogModel;
using Edam.Data.CatalogDb;
using Edam.UI.Catalog.Models;
using Edam.UI.CatalogExplorer;

namespace Edam.UI.Catalog.Controls;

public class CatalogViewModel
{

    public const string CATALOG_INITIALIZED = "CATALOG-INITIALIZED";

    public AppModelState State = null;
    public bool HasCatalog = false;

    public CatalogInfo? Catalog = null;
    public CatalogItem RootItem = null;

    public NotificationEventHandler NotifyEvent { get; set; }

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async Task GetCatalogAsync(AppModelState state)
    {
        string connectionUri = state.GetConnectionUri();

        Catalog = await CatalogServiceHelper.GetCatalogAsync(
            connectionUri);

    }

    /// <summary>
    /// Post Item.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public async Task<ItemDataInfo> PostItem(string path, byte[] payload)
    {
        ItemDataInfo? itemData = null;
        CatalogTreeBuilder builder =
            new CatalogTreeBuilder(Catalog.CatalogService, Catalog);
        var item = await builder.GetItemAsync(path);
        if (payload != null && payload.Length > 0)
        {
            itemData = item.ToItemData(payload);
            var rItem = await Catalog.CatalogService.AddItemAsync(itemData);
        }
        return itemData;
    }

}
