using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edam.Application;
using Edam.Data.CatalogModel;
using Edam.Data.CatalogDb;
using Edam.Diagnostics;
using Windows.Storage.Pickers;

namespace Edam.UI.CatalogExplorer;

public class CatalogServiceHelper
{
    public static string _SessionId = Guid.NewGuid().ToString();

    /// <summary>
    /// Get Catalog Service instance...
    /// </summary>
    /// <returns>Catalog Service instance is returned</returns>
    public static ICatalogService GetInstance(string? connectionString = null)
    {
        var _conString = String.IsNullOrWhiteSpace(connectionString) ?
            AppSettings.GetConnectionString("fileSystemDb") :
            connectionString;

        // initialize repository
        CatalogInstance instance = new CatalogInstance();
        ResultsLog<ICatalogService?> results = instance.GetCatalog(
           CatalogInstance.EDAM_FILE_SYSTEM_DB, connectionString);
        if (results.Success)
        {
            results.Instance.SetContainer(_SessionId, "");
            return results.Instance;
        }
        return null;
    }

    /// <summary>
    /// Get Instance Async...
    /// </summary>
    /// <param name="connectionString">connection string</param>
    /// <returns>instance is returned</returns>
    public static async Task<ICatalogService> GetInstanceAsync(
        string? connectionString = null)
    {
        ICatalogService instance = null;
        await Task.Run(() => {
            instance = GetInstance(connectionString);
        });
        return instance;
    }

    /// <summary>
    /// Get Catalog to build its tree and access data.
    /// </summary>
    /// <param name="connectionString">connection string</param>
    /// <returns>instance of catalog is returned</returns>
    public static CatalogInfo GetCatalog(string? connectionString = null)
    {
        ICatalogService instance = GetInstance(connectionString);
        CatalogInfo catalog = new CatalogInfo(instance, _SessionId);
        catalog.InitializeCatalog("", buildTree: true);
        return catalog;
    }

    /// <summary>
    /// Get Catalog Async...
    /// </summary>
    /// <param name="connectionString">connection string</param>
    /// <returns>instance of catalog is returned</returns>
    public static async Task<CatalogInfo> GetCatalogAsync(
        string? connectionString = null)
    {
        CatalogInfo catalog = null;
        await Task.Run(() => {
            catalog = GetCatalog(connectionString);
        });
        return catalog;
    }
}
