﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Edam.Application;
using Edam.Data.CatalogModel;
using Edam.DataObjects.Medias;
using Edam.DataObjects.Requests;

namespace Edam.Data.CatalogDb;

/// <summary>
/// Support for File System repository inqueries and requests.
/// </summary>
public class CatalogServiceInstance : ICatalogService, IDisposable
{

   #region -- 1.00 - Fields and Properties

   public const string DELETED = "Deleted";
   public const string ROOT_ID = "[root]";
   public const string ROOT_PATH = "/";

   private CatalogContext? DbContext { get; set; } = null;

   public ContainerInfo? DefaultContainer { get; set; } = null;
   public ContainerInfo? CurrentContainer { get; set; } = null;

   public string? _defaultConnectionString = null;

   #endregion
   #region -- 1.50 - Initialization and Disposition

   public CatalogServiceInstance(string? defaultConnectionString)
   {
      _defaultConnectionString = defaultConnectionString;
   }

   /// <summary>
   /// Initialize Repository
   /// </summary>
   private void InitializeDbContext()
   {
      var connectionString = 
         String.IsNullOrWhiteSpace(_defaultConnectionString) ?
            AppSettings.GetConnectionString("fileSystemDb") :
            _defaultConnectionString;

      DbContext = new CatalogContext(connectionString);
      if (!DbContext.Database.CanConnect())
      {
         try
         {
            DbContext.Database.EnsureCreated();
         }
         catch (Exception ex)
         {

         }
      }

      if (!DbContext.ContentTypes.Any())
      {
         var types = new ContentTypeInfo[]
         {
         new ContentTypeInfo(MediaContentTypeHelper.JSONLD, "json-ld document"),
         new ContentTypeInfo(MediaContentTypeHelper.JsonDocument,
            "json document"),
         new ContentTypeInfo(MediaContentTypeHelper.XmlDocument, "xml text"),
         new ContentTypeInfo(MediaContentTypeHelper.TextFile, "text document"),
         new ContentTypeInfo(MediaContentTypeHelper.OfficeExcelXmlFile,
            "excel open xml document"),
         new ContentTypeInfo(MediaContentTypeHelper.JAVASCRIPT,
            "javascript document")
         };
         foreach (var type in types)
         {
            DbContext.ContentTypes.Add(type);
         }
         DbContext.SaveChanges();
      }

      // define default container
      if (!DbContext.Containers.Any())
      {
         DefaultContainer = new ContainerInfo();
         DbContext.Containers.Add(DefaultContainer);
         DbContext.SaveChanges();
      }
      else
      {
         DefaultContainer = GetContainer(null);
      }
      CurrentContainer = DefaultContainer;

      // define default container root item
      if (!DbContext.FileItems.Any())
      {
         CreateRootItem();
      }

   }

   /// <summary>
   /// Release resources.
   /// </summary>
   public void Dispose()
   {
      if (DbContext != null)
      {
         DbContext.Dispose();
         DbContext = null;
      }
   }

   #endregion
   #region -- 4.00 - Find Containers, Items, or Item-Data Methods

   /// <summary>
   /// Get Root Item of given container.
   /// </summary>
   /// <param name="containerId">container id</param>
   /// <returns>instance of File-Item is returned</returns>
   public ItemInfo GetContainerRootItem(Guid id)
   {
      var items = from item in DbContext.FileItems
                  where item.Name == ROOT_ID &&
                        item.FullPath == ROOT_PATH &&
                        item.ContainerId == id
                  select item;
      var l = items.ToList();
      return l.Count > 0 ? l[0] : null;
   }

   /// <summary>
   /// Get Root Item of given container.
   /// </summary>
   /// <param name="containerId">container id</param>
   /// <returns>instance of File-Item is returned</returns>
   public ContainerInfo GetContainer(Guid id)
   {
      var items = from item in DbContext.Containers
                  where item.Id == id
                  select item;
      var l = items.ToList();
      return l.Count > 0 ? l[0] : null;
   }

   /// <summary>
   /// Get Containers.
   /// </summary>
   /// <returns>list of containers is returned</returns>
   public List<ContainerInfo> GetContainers()
   {
      return DbContext.Containers.ToList<ContainerInfo>();
   }

   /// <summary>
   /// Get Container Items.
   /// </summary>
   /// <param name="id">Container ID</param>
   /// <returns></returns>
   public List<ItemInfo> GetContainerItems(Guid id)
   {
      var items = from x in DbContext.FileItems
                  where x.ContainerId == id
                  select x;
      return items.ToList<ItemInfo>();
   }

   /// <summary>
   /// Get Item by its unique full path.
   /// </summary>
   /// <param name="path">unique full path</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public ItemInfo GetItemByPath(string path)
   {
      var ditems = from x in DbContext.FileItems
                   where x.FullPath == path &&
                         x.ContainerId == CurrentContainer.Id
                   select x;
      if (ditems.Any())
      {
         return ditems.ToList<ItemInfo>()[0];
      }
      return null;
   }

   /// <summary>
   /// Get Item Data entries.
   /// </summary>
   /// <param name="itemId">File-Item ID</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public List<ItemDataInfo> GetItemDataByItemId(Guid itemId)
   {
      var ditems = from x in DbContext.DataItems
                   where x.ItemId == itemId
                   select x;
      return ditems.ToList<ItemDataInfo>();
   }

   /// <summary>
   /// Get Item Data entries.
   /// </summary>
   /// <param name="id">File-Item ID</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public List<ItemDataInfo> GetItemData(Guid id)
   {
      var ditems = from x in DbContext.DataItems
                   where x.Id == id
                   select x;
      return ditems.ToList<ItemDataInfo>();
   }

   /// <summary>
   /// Get Item Data entries.
   /// </summary>
   /// <param name="id">File-Item ID</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public ItemDataInfo GetData(Guid id)
   {
      var ditems = from x in DbContext.DataItems
                   where x.Id == id
                   select x;
      var list = ditems.ToList<ItemDataInfo>();
      return list.Count > 0 ? list[0] : null;
   }

   /// <summary>
   /// Get Item Data by name.
   /// </summary>
   /// <param name="fileItemId">File-Item ID</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public ItemDataInfo GetDataByName(Guid fileItemId, string name)
   {
      var ditems = from x in DbContext.DataItems
                   where x.ItemId == fileItemId &&
                         x.Name == name
                   select x;
      var list = ditems.ToList<ItemDataInfo>();
      return list.Count > 0 ? list[0] : null;
   }

   /// <summary>
   /// Get Item.
   /// </summary>
   /// <param name="id">File-Item ID</param>
   /// <returns>Get list of data items for given file-item id</returns>
   public ItemInfo? GetItem(Guid id)
   {
      var ditems = from x in DbContext.FileItems
                   where x.Id == id
                   select x;
      var list = ditems.ToList<ItemInfo>();
      return list.Count > 0 ? list[0] : null;
   }

   /// <summary>
   /// Get Branch items (of current container).
   /// </summary>
   /// <param name="path">path to fetch first level items</param>
   /// <returns>Get list of items for given partial path</returns>
   public List<ItemInfo?> GetBranch(string? path = null)
   {
      var spath = String.IsNullOrWhiteSpace(path) ? "/" : path;
      var ditems = DbContext.FileItems.
         Where((x) => EF.Functions.Like(x.FullPath, spath +"%") &&
            x.Container.Id == CurrentContainer.Id);

      var list = ditems.ToList<ItemInfo>();
      return list;
   }

   /// <summary>
   /// Get Content Type.
   /// </summary>
   /// <param name="id">Container ID</param>
   /// <returns></returns>
   public ContentTypeInfo GetContentType(string contentTypeId)
   {
      var items = from x in DbContext.ContentTypes
                  where x.TypeId == contentTypeId
                  select x;
      var list = items.ToList<ContentTypeInfo>();
      return list.Count > 0 ? list[0] : null;
   }

   #endregion
   #region -- 4.00 - Container Support

   /// <summary>
   /// Get Container ID.
   /// </summary>
   /// <param name="containerId"></param>
   /// <returns></returns>
   private string GetContainerId(string? containerId)
   {
      if (String.IsNullOrWhiteSpace(containerId))
      {
         containerId = ContainerInfo.CONTAINER_ID_DEFAULT;
      }
      return containerId;
   }

   /// <summary>
   /// Get Container.
   /// </summary>
   /// <param name="containerId"></param>
   /// <param name="checkId">true to check ID</param>
   /// <returns></returns>
   public ContainerInfo? GetContainer(string? containerId, bool checkId = true)
   {
      var cid = checkId ? GetContainerId(containerId) : containerId;
      ContainerInfo container = DbContext.Containers.Where(
         x => x.ContainerId == cid).FirstOrDefault();
      return container;
   }

   /// <summary>
   /// Set Session and current container (by id).
   /// </summary>
   /// <param name="sessionId">(optional) session id</param>
   /// <param name="containerId">(optional) container id</param>
   /// <returns>found container is is returned</returns>
   public virtual ContainerInfo? SetContainer(
      string sessionId, string containerId)
   {
      var cid = GetContainerId(containerId);

      ContainerInfo container = CurrentContainer;

      if (DbContext == null)
      {
         InitializeDbContext();
         container = CurrentContainer;
         if (containerId == "\"\"")
         {
            cid = container.ContainerId;
         }
      }

      if (container == null || container.ContainerId != cid)
      {
         container = GetContainer(cid);
      }

      CurrentContainer = container;
      if (DefaultContainer == null)
      {
         DefaultContainer = container;
      }
      return container;
   }

   /// <summary>
   /// Enlist (Add or Update) container info.
   /// </summary>
   /// <param name="containerId">container id</param>
   /// <param name="description">description</param>
   /// <returns>container info is returned</returns>
   public ContainerInfo EnlistContainer(string containerId, string description)
   {
      var container = GetContainer(containerId);
      if (container == null || container.ContainerId != containerId)
      {
         // prepare container
         container = new();
         container.ContainerId = containerId;
         container.Description = description;
         DbContext.Containers.Add(container);

         // add root item
         CreateRootItem(container.Id);
      }
      else if (container.Description != description)
      {
         container.Description += description;
         DbContext.SaveChanges();
      }

      // new container is the current container...
      CurrentContainer = container;

      return container;
   }

   /// <summary>
   /// Delete Container Item-Data and Items.
   /// </summary>
   /// <param name="id">Container ID</param>
   private void DeleteContainerData(Guid id)
   {
      var fitems = GetContainerItems(id);
      foreach(var item in fitems)
      { 
         var ditems = GetItemData(item.Id);
         foreach(var ditem in ditems)
         {
            DbContext.DataItems.Remove(ditem);
         }
         DbContext.FileItems.Remove(item);
      }
      DbContext.SaveChanges();
   }

   /// <summary>
   /// Delist (Delete) container info.
   /// </summary>
   /// <param name="containerId">container id</param>
   /// <returns>container info is returned</returns>
   public ContainerInfo DelistContainer(string containerId)
   {
      if (String.IsNullOrWhiteSpace(containerId))
      {
         return null;
      }

      var container = GetContainer(containerId, checkId: false);
      if (container != null || container.ContainerId == containerId)
      {
         // delete all container items...
         DeleteContainerData(container.Id);

         // finally... delete container
         container.StatusCode = DELETED;
         DbContext.Remove(container);
         DbContext.SaveChanges();
      }
      return container;
   }

   /// <summary>
   /// Get Current Container.
   /// </summary>
   /// <returns>instance of container is returned</returns>
   public ContainerInfo GetCurrentContainer()
   {
      if (CurrentContainer == null)
      {
         if (DefaultContainer == null)
         {
            GetContainer(null);
         }
      }

      return CurrentContainer;
   }

   #endregion
   #region -- 4.00 - File Item Branch - Leaf Support

   /// <summary>
   /// Add item (or update) in repository based on its full path.
   /// </summary>
   /// <param name="item">add item</param>
   public ItemInfo AddItem(ItemInfo item)
   {
      ItemInfo ritem;
      var iitem = GetItemByPath(item.FullPath);
      if (iitem == null)
      {
         ritem = item;
         DbContext.FileItems.Add(item);
      }
      else if (item.Id != iitem.Id)
      {
         item.Id = iitem.Id;
         ritem = iitem;
         DbContext.FileItems.Update(iitem);
      }
      else
      {
         ritem = iitem;
         return ritem;
      }
      DbContext.SaveChanges();
      return ritem;
   }

   /// <summary>
   /// Delete Item-Data and Item.
   /// </summary>
   /// <param name="id">Container ID</param>
   /// <returns>request status (Completed) is returned, else (Unknown)</returns>
   public RequestStatus DeleteItem(Guid id)
   {
      RequestStatus status = RequestStatus.Unknown;
      var item = GetItem(id);
      if (item != null)
      {
         var ditems = GetItemData(item.Id);
         foreach (var ditem in ditems)
         {
            DbContext.DataItems.Remove(ditem);
         }
         DbContext.FileItems.Remove(item);
         DbContext.SaveChanges();
         status = RequestStatus.Completed;
      }
      return status;
   }

   /// <summary>
   /// Create container root item.
   /// </summary>
   /// <param name="containerId">(optional) container id, else the container Id
   /// of the current container is used</param>
   public ItemInfo CreateRootItem(Guid? containerId = null)
   {
      Guid cid = containerId == null ? CurrentContainer.Id : containerId.Value;
      var root = GetContainerRootItem(cid);

      ItemInfo item = null;
      if (root == null)
      {
         item = CreateBranch(ROOT_ID, "root", cid);
         item.ContainerId = cid;
         item.FullPath = ROOT_PATH;

         DbContext.FileItems.Add(item);
         DbContext.SaveChanges();
      }
      else
      {
         item = root;
      }
      return item;
   }

   /// <summary>
   /// Create Branch.
   /// </summary>
   /// <param name="name">name of branch</param>
   /// <param name="description">description</param>
   /// <param name="containerId">container id [default: CurrentContainer.Id]
   /// </param>
   /// <returns>file item instance is returned</returns>
   public ItemInfo CreateBranch(
      string name, string? description = null, Guid? containerId = null)
   {
      var desc = description ?? name;
      var item = new ItemInfo();

      if (containerId == null)
      {
         item.ContainerId = CurrentContainer.Id;
         item.Container = CurrentContainer;
      }
      else
      {
         var container = GetContainer(containerId.Value);
         item.Container = container;
         item.ContainerId = containerId.Value;
      }

      item.ItemType = DataObjects.Trees.TreeItemType.Branch;
      item.Name = name;
      item.Description = desc;
      item.FullPath = String.Empty;
      return item;
   }

   /// <summary>
   /// Create/Update Leaf.
   /// </summary>
   /// <param name="path">full path</param>
   /// <param name="name">name of branch</param>
   /// <param name="description">description</param>
   /// <returns>file item instance is returned</returns>
   public ItemInfo CreateLeaf(string path, string name,
      Guid? id = null, string? description = null, string? dataValue = null)
   {
      if (String.IsNullOrWhiteSpace(path))
      {
         path = "//Archive";
      }

      var desc = description ?? name;
      var item = new ItemInfo();

      item.Id = id ?? item.Id;
      item.ContainerId = CurrentContainer.Id;
      item.Container = CurrentContainer;

      item.ItemType = DataObjects.Trees.TreeItemType.Leaf;
      item.FullPath = path;
      item.Name = name;
      item.Description = desc;
      item.FullPath = String.Empty;

      if (dataValue != null)
      {
         CreateDataLeaf(item, name, null, dataValue);
      }

      return item;
   }

   #endregion
   #region -- 4.00 - File Item Data Leaf Support

   /// <summary>
   /// Add given data item (or update).  Understand that a data item is uniquely
   /// identified by its name.
   /// </summary>
   /// <param name="item">add item</param>
   public ItemDataInfo AddItem(ItemDataInfo item)
   {
      ItemDataInfo ritem;
      var ditem = GetDataByName(item.ItemId, item.Name);
      if (ditem == null)
      {
         ritem = item;
         DbContext.DataItems.Add(item);
      }
      else if (item.Id != ditem.Id)
      {
         item.Id = ditem.Id;
         ritem = ditem;
         DbContext.DataItems.Update(ditem);
      }
      else
      {
         ritem = ditem;
         return ritem;
      }
      DbContext.SaveChanges();
      return ritem;
   }

   /// <summary>
   /// Create/Update file item data.
   /// </summary>
   /// <param name="item">parent file item</param>
   /// <param name="name">unique data item name</param>
   /// <param name="dataId">data id</param>
   /// <param name="dataValue">data value</param>
   /// <returns>file item instance is returned</returns>
   public ItemDataInfo CreateDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null)
   {
      var data = ItemDataInfo.CreateDataLeaf(item.Id, name, dataId);
      data.Data = dataValue;

      return data;
   }

   /// <summary>
   /// Create/Update file item data.
   /// </summary>
   /// <param name="item">parent file item</param>
   /// <param name="name">unique data item name</param>
   /// <param name="dataId">data id</param>
   /// <param name="dataValue">data value</param>
   /// <returns>file item instance is returned</returns>
   public ItemDataInfo CreateDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, string dataValue = null)
   {
      var data = ItemDataInfo.CreateDataLeaf(item.Id, name, dataId);
      data.DataText = dataValue;
      return data;
   }

   /// <summary>
   /// Create/Update file item data.
   /// </summary>
   /// <param name="item">parent file item</param>
   /// <param name="name">unique data item name</param>
   /// <param name="dataId">data id</param>
   /// <param name="dataValue">data value</param>
   /// <returns>file item instance is returned</returns>
   public ItemDataInfo AddDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, byte[] dataValue = null)
   {
      var data = CreateDataLeaf(item, name, dataId, dataValue);
      return AddItem(data);
   }

   /// <summary>
   /// Create/Update file item data.
   /// </summary>
   /// <param name="item">parent file item</param>
   /// <param name="name">unique data item name</param>
   /// <param name="dataId">data id</param>
   /// <param name="dataValue">data value</param>
   /// <returns>file item instance is returned</returns>
   public ItemDataInfo AddDataLeaf(ItemInfo item, string name,
      Guid? dataId = null, string dataValue = null)
   {
      var data = CreateDataLeaf(item, name, dataId, dataValue);
      return AddItem(data);
   }

   /// <summary>
   /// Delete Item-Data and Item.
   /// </summary>
   /// <param name="itemId">Item ID</param>
   /// <returns>request status (Completed) is returned, else (Unknown)</returns>
   public RequestStatus DeleteData(Guid id)
   {
      RequestStatus status = RequestStatus.Unknown;
      var ditem = GetData(id);
      if (ditem != null)
      {
         DbContext.DataItems.Remove(ditem);
         DbContext.SaveChanges();
         status = RequestStatus.Completed;
      }
      return status;
   }

   /// <summary>
   /// Delete Item-Data and Item.
   /// </summary>
   /// <param name="itemId">Item ID</param>
   /// <returns>request status (Completed) is returned, else (Unknown)</returns>
   public RequestStatus DeleteItemData(Guid itemId)
   {
      RequestStatus status = RequestStatus.Unknown;
      var item = GetItem(itemId);
      if (item != null)
      {
         var ditems = GetItemData(item.Id);
         foreach (var ditem in ditems)
         {
            DbContext.DataItems.Remove(ditem);
         }
         DbContext.SaveChanges();
         status = RequestStatus.Completed;
      }
      return status;
   }

   #endregion

}