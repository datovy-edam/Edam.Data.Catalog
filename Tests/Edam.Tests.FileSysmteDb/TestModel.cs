using Edam.Data.CatalogModel;
using Edam.Diagnostics;
using Edam.InOut;
using Edam.Tests.FileSysmteDb;
using System.Runtime.CompilerServices;

namespace Edam.Test.FileSystemDb;

[TestClass]
public class TestModel
{

   [TestMethod]
   public void TestContextInitialization()
   {
      // initialize repository
      ICatalogService instance = CommonHelpers.GetInstance();
      TestPathParsing();
   }

   [TestMethod]
   public void TestPathParsing()
   {
      FileItemInfo fitem = new FileItemInfo();
      fitem.FullPath = CommonHelpers.JSON_FILE_PATH;
      CatalogPathItem item = new CatalogPathItem(fitem);

      fitem.FullPath = CommonHelpers.JSON_DATA_PATH;
      item = new CatalogPathItem(fitem);
   }

   /// <summary>
   /// This will build a tree if one exists for the requested container that
   /// since is not specified is the "default" container.
   /// </summary>
   [TestMethod]
   public void TestTreeBuilder()
   {
      CatalogInfo catalog = CommonHelpers.GetCatalog();
      catalog.InitializeCatalog("", true);
   }

   /// <summary>
   /// The following will create catalog items (files) entried if those don't
   /// exists.  Then retrieve an entry if it exists.
   /// </summary>
   [TestMethod]
   public void TestPathTreeBuilder()
   {
      CatalogInfo catalog = CommonHelpers.GetCatalog();

      // get a tree builder
      CatalogTreeBuilder builder = 
         new CatalogTreeBuilder(catalog.CatalogService, catalog);

      // add path items (User, esobr, Documents, and Edam.Studio)
      var pitem = builder.GetItem(CommonHelpers.JSON_FILE_PATH);

      // add same items and ending leaf (coco.json)
      pitem = builder.GetItem(CommonHelpers.JSON_DATA_PATH);

      // add leaf data...
      var dataLeaf = catalog.CatalogService.AddDataLeaf(
         pitem.Item, "TEST", null, CommonHelpers.FILE_TEST_DATA);

      // remove item and related data for JSON_FILE_PATH...
      catalog.CatalogService.DeleteItem(pitem.Item.Id);
   }

   [TestMethod]
   public void TestBranch()
   {
      CatalogInfo catalog = CommonHelpers.GetCatalog();

      // get a tree builder
      CatalogTreeBuilder builder =
         new CatalogTreeBuilder(catalog.CatalogService, catalog);

      builder.GetBranch("/");
      builder.GetBranch(CommonHelpers.JSON_BRANCH_PATH);
      builder.BuildTree();
      builder.GetBranch("");
   }

}
