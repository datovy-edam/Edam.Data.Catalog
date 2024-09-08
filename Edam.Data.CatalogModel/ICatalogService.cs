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

   FileItemInfo CreateRootItem(Guid? containerId = null);
   FileItemInfo CreateBranch(string name, string? description = null, 
      Guid? containerId = null);
   FileItemInfo CreateLeaf(string path, string name,
      Guid? id = null, string? description = null, string? dataValue = null);

   FileItemDataInfo CreateDataLeaf(FileItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null);
   FileItemDataInfo CreateDataLeaf(FileItemInfo item, string name,
      Guid? dataId = null, string dataValue = null);

   FileItemDataInfo AddDataLeaf(FileItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null);
   FileItemDataInfo AddDataLeaf(FileItemInfo item, string name,
      Guid? dataId = null, string dataValue = null);

   FileItemInfo GetContainerRootItem(Guid id);
   List<ContainerInfo> GetContainers();
   List<FileItemInfo> GetContainerItems(Guid id);
   ContentTypeInfo GetContentType(string contentTypeId);
   FileItemDataInfo GetDataByName(Guid fileItemId, string name);

   FileItemInfo? GetItem(Guid id);
   FileItemInfo GetItemByPath(string name);
   void DeleteItem(Guid id);

   List<FileItemInfo?> GetBranch(string? path = null);

   FileItemInfo AddItem(FileItemInfo item);

   FileItemDataInfo AddItem(FileItemDataInfo item);
   FileItemDataInfo GetData(Guid id);
   List<FileItemDataInfo> GetItemData(Guid id);
   void DeleteItemData(Guid id);
   void DeleteData(Guid id);

}
