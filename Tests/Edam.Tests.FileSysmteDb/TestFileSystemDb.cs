using Edam.Data.FileSystemDb;
using Edam.Data.FileSystemModel;
using Edam.Diagnostics;
using Edam.InOut;
using System.Runtime.CompilerServices;

namespace Edam.Test.FileSystemDb;

[TestClass]
public class TestModel
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
      fitem.FullPath = JSON_FILE_PATH;
      CatalogPathItem item = new CatalogPathItem(fitem);

      fitem.FullPath = JSON_DATA_PATH;
      item = new CatalogPathItem(fitem);
   }

   /// <summary>
   /// This will build a tree if one exists for the requested container that
   /// since is not specified is the "default" container.
   /// </summary>
   [TestMethod]
   public void TestTreeBuilder()
   {
      CatalogInfo catalog = GetCatalog();
      catalog.InitializeCatalog("", true);
   }

   /// <summary>
   /// The following will create catalog items (files) entried if those don't
   /// exists.  Then retrieve an entry if it exists.
   /// </summary>
   [TestMethod]
   public void TestPathTreeBuilder()
   {
      CatalogInfo catalog = GetCatalog();

      // get a tree builder
      CatalogTreeBuilder builder = 
         new CatalogTreeBuilder(catalog.CatalogService, catalog);

      // add path items (User, esobr, Documents, and Edam.Studio)
      var pitem = builder.GetItem(JSON_FILE_PATH);

      // add same items and ending leaf (coco.json)
      pitem = builder.GetItem(JSON_DATA_PATH);

      // add leaf data...
      var dataLeaf = catalog.CatalogService.AddDataLeaf(
         pitem.Item, "TEST", null, FILE_TEST_DATA);

      // remove item and related data for JSON_FILE_PATH...
      catalog.CatalogService.DeleteItem(pitem.Item.Id);
   }

   [TestMethod]
   public void TestBranch()
   {
      CatalogInfo catalog = GetCatalog();

      // get a tree builder
      CatalogTreeBuilder builder =
         new CatalogTreeBuilder(catalog.CatalogService, catalog);

      builder.GetBranch("/");
      builder.GetBranch(JSON_BRANCH_PATH);
      builder.BuildTree();
      builder.GetBranch("");
   }

}
