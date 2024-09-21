using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.Data.CatalogModel;

public interface ICatalogService
{

   ContainerInfo DefaultContainer { get; set; }
   ContainerInfo CurrentContainer { get; set; }

   ContainerInfo GetContainer(string? containerId, bool checkId = true);
   ContainerInfo GetContainer(Guid id);
   ContainerInfo SetContainer(string sessionId, string containerId);
   ContainerInfo EnlistContainer(string containerId, string description);
   ContainerInfo DelistContainer(string containerId);

   ItemInfo CreateRootItem(Guid? containerId = null);
   ItemInfo CreateBranch(string name, string? description = null, 
      Guid? containerId = null);
   ItemInfo CreateLeaf(string path, string name,
      Guid? id = null, string? description = null, string? dataValue = null);

   ItemDataInfo CreateDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null);
   ItemDataInfo CreateDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, string dataValue = null);

   ItemDataInfo AddDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null);
   ItemDataInfo AddDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, string dataValue = null);

   ItemInfo GetContainerRootItem(Guid id);
   List<ContainerInfo> GetContainers();
   List<ItemInfo> GetContainerItems(Guid id);
   ContentTypeInfo GetContentType(string contentTypeId);
   ItemDataInfo GetDataByName(Guid fileItemId, string name);

   ItemInfo? GetItem(Guid id);
   ItemInfo GetItemByPath(string name);
   void DeleteItem(Guid id);

   List<ItemInfo?> GetBranch(string? path = null);

   ItemInfo AddItem(ItemInfo item);

   ItemDataInfo AddItem(ItemDataInfo item);
   ItemDataInfo GetData(Guid id);
   List<ItemDataInfo> GetItemData(Guid id);
   void DeleteItemData(Guid id);
   void DeleteData(Guid id);

}
