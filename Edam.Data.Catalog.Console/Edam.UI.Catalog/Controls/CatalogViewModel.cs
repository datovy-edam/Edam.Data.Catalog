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

}
