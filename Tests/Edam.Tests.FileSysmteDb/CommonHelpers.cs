using Edam.Data.FileSystemDb;
using Edam.Data.CatalogModel;
using Edam.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.Tests.FileSysmteDb;

public class CommonHelpers
{
   public const string JSON_BRANCH_PATH = "J:/Edam.Studio";
   public const string JSON_FILE_PATH =
      "J:/Edam.Studio/Projects/HealthCare/Documents";
   public const string JSON_DATA_PATH =
      "J:/Edam.Studio/Projects/HealthCare/Documents/schema.json";
   public const string FILE_TEST_DATA = "{ title: \"HAPPY happy life...\" }";

   public static string _SessionId = Guid.NewGuid().ToString();
   public static ICatalogService GetInstance()
   {
      // initialize repository
      CatalogInstance catalogInstance = new CatalogInstance();
      ResultsLog<ICatalogService> results = catalogInstance.GetCatalog(
         CatalogInstance.EDAM_FILE_SYSTEM_DB);
      if (results.Success)
      {
         results.Instance.SetContainer(_SessionId, "");
         return results.Instance;
      }
      return null;
   }

   /// <summary>
   /// Get Catalog to build its tree and access data.
   /// </summary>
   /// <returns>instance of catalog is returned</returns>
   public static CatalogInfo GetCatalog()
   {
      ICatalogService instance = GetInstance();
      CatalogInfo catalog = new CatalogInfo(instance, _SessionId);
      catalog.InitializeCatalog("");
      return catalog;
   }

}
