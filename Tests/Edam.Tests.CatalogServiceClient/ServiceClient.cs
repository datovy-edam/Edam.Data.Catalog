using Edam.Data.CatalogModel;
using Edam.Data.CatalogServiceClient;

namespace Edam.Tests.CatalogServiceClient;

[TestClass]
public class ServiceClient
{

   private string _sessionId;

   [TestInitialize]
   public void InitializeAll()
   {
      _sessionId = Guid.NewGuid().ToString();
   }

   [TestMethod]
   public void Initialize()
   {
      CatalogClient client = CatalogClient.InitializeClient(_sessionId);
   }

   [TestMethod]
   public void GetSetContainer()
   {
      CatalogClient client = CatalogClient.InitializeClient(_sessionId);
   }

   [TestMethod]
   public void AddItem()
   {
      CatalogClient client = CatalogClient.InitializeClient(_sessionId);
      ItemInfo item = new ItemInfo
      {
         Name = "sample",
         Description = "sample leaf",
         ContainerId = client.DefaultContainer.Id,
         FullPath = "c:/projects/HC_Surveillance/arguments/sample.json",
         Id = Guid.NewGuid(),
         ItemType = DataObjects.Trees.TreeItemType.Leaf
      };

      var citem = client.AddItem(item);
   }

   [TestMethod]
   public void GetContentType()
   {
      CatalogClient client = CatalogClient.InitializeClient(_sessionId);
      client.GetContentType("application/json");
   }

}
