using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Edam.DataObjects.Requests;

namespace Edam.Data.CatalogModel;

public interface ICatalogService
{

   ContainerInfo DefaultContainer { get; set; }
   ContainerInfo CurrentContainer { get; set; }

   ContainerInfo GetContainer(string? containerId, bool checkId = true);
   ContainerInfo GetContainer(Guid containerId);
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

   ItemInfo GetContainerRootItem(Guid containerId);
   List<ContainerInfo> GetContainers();
   List<ItemInfo> GetContainerItems(Guid icontainerIdd);
   ContentTypeInfo GetContentType(string contentTypeId);
   ItemDataInfo GetDataByName(Guid itemId, string name);

   ItemInfo? GetItem(Guid itemId);
   ItemInfo GetItemByPath(string name);
   RequestStatus DeleteItem(Guid itemId);

   List<ItemInfo?> GetBranch(string? path = null);

   ItemInfo AddItem(ItemInfo item);
   ItemDataInfo AddItem(ItemDataInfo item);

   ItemDataInfo GetData(Guid dataId);
   List<ItemDataInfo> GetItemData(Guid itemId);
   RequestStatus DeleteItemData(Guid itemId);
   RequestStatus DeleteData(Guid dataId);

}
