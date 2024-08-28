using Edam.Data.FileSystemDb;
using Edam.Data.FileSystemModel;
using Edam.Diagnostics;
using Edam.InOut;
using System.Runtime.CompilerServices;

namespace Edam.Test.FileSystemDb
{

   [TestClass]
   public class TestModel
   {
      private string _SessionId = Guid.NewGuid().ToString();
      private ICatalogService GetInstance()
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
      private CatalogInfo GetCatalog()
      {
         ICatalogService instance = GetInstance();
         CatalogInfo catalog = new CatalogInfo(instance, _SessionId);
         catalog.InitializeCatalog("");
         return catalog;
      }

      [TestMethod]
      public void TestContextInitialization()
      {
         // initialize repository
         ICatalogService instance = GetInstance();
         TestPathParsing();
      }

      [TestMethod]
      public void TestPathParsing()
      {
         FileItemInfo fitem = new FileItemInfo();
         fitem.FullPath = "C:/Users/esobr/Documents/Edam.Studio";
         CatalogPathItem item = new CatalogPathItem(fitem);

         fitem.FullPath = "C:/Users/esobr/Documents/Edam.Studio/coco.json";
         item = new CatalogPathItem(fitem);
      }

      [TestMethod]
      public void TestTreeBuilder()
      {
         CatalogInfo catalog = GetCatalog();
         catalog.InitializeCatalog("", true);
      }

      [TestMethod]
      public void TestPathTreeBuilder()
      {
         CatalogInfo catalog = GetCatalog();

         CatalogTreeBuilder builder = 
            new CatalogTreeBuilder(catalog.CatalogService, catalog);
         var pitem = builder.GetItem("C:/Users/esobr/Documents/Edam.Studio");
         pitem = builder.GetItem(
            "C:/Users/esobr/Documents/Edam.Studio/coco.json");
      }
   }

}

