using Edam.Data.CatalogModel;
using Edam.Tests.FileSysmteDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.Test.FileSysmteDb;

/// <summary>
/// Upon Catalog Services database initialization a 'default' container is
/// created.  You can rely on the default container as a fallback just because
/// we are lazy (also) by default.
/// </summary>
[TestClass]
public class TestContainers
{

   // initialize repository
   private CatalogInfo _Catalog = CommonHelpers.GetCatalog();

   private ICatalogService _Instance
   {
      get { return _Catalog.CatalogService; }
   }

   [TestMethod]
   public void TestInitializeContainer()
   {
      // get the list of containers
      var containers = _Catalog.CatalogService.GetContainers();
   }

   [TestMethod]
   public void TestEnlisting()
   {
      string pprojects = "PrivateProjects";

      // (here elist = add)
      var container = _Instance.EnlistContainer(
         pprojects, "Private Projects");

      // get the list of containers
      var containers = _Catalog.CatalogService.GetContainers();

      // (here delist = remove)
      var dcontainer = _Instance.DelistContainer(pprojects);
   }

}
