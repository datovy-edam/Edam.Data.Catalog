﻿using Edam.DataObjects.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Edam.Data.FileSystemModel;

/// <summary>
/// Catalog Tree Builder based on the common TreeItem.
/// </summary>
public class CatalogTreeBuilder
{
   private int _ItemCount = 0;

   private ICatalogService _Service;
   private CatalogInfo _CatalogInfo;
   private Dictionary<string, CatalogPathItem>? _Dictionary = 
      new Dictionary<string, CatalogPathItem>();

   public CatalogTreeBuilder(ICatalogService service, CatalogInfo catalog)
   { 
      _Service = service;
      _CatalogInfo = catalog;
   }

   /// <summary>
   /// Create item.
   /// </summary>
   /// <param name="pathItem">path item information to create new item</param>
   /// <returns>return the created item</returns>
   public static CatalogItemInfo CreateItem(CatalogPathItem pathItem)
   {
      // create a new catalog item
      CatalogItemInfo citem = new CatalogItemInfo
      {
         Name = pathItem.NameFull,
         Title = pathItem.Item.Description,
         Tag = pathItem
      };

      if (pathItem.Item.ItemType == TreeItemType.Branch)
      {
         citem.Type = TreeItemType.Branch;
         citem.Icon = CatalogInfo.IconBranch;
      }
      else
      {
         citem.Type = TreeItemType.Leaf;
         citem.Icon = CatalogInfo.IconLeaf;
      }

      string[] items = pathItem.Full.Split('/');
      citem.Level = new short[items.Length];
      for (int i = 0; i < items.Length; i++)
      {
         citem.Level[i] = (short)i;
      }

      pathItem.TreeItem = citem;

      return citem;
   }

   /// <summary>
   /// Create a File Item and register id.
   /// </summary>
   /// <param name="item"></param>
   /// <param name="path"></param>
   /// <returns></returns>
   private FileItemInfo CreateFileItem(CatalogItemInfo item, string path)
   {
      FileItemInfo pitem = new FileItemInfo();
      pitem.Name = item.Name;
      pitem.Description = item.Title;
      pitem.ItemType = item.Type == TreeItemType.Branch ?
         TreeItemType.Branch : TreeItemType.Leaf;
      pitem.FullPath = path;
      pitem.Container = _Service.CurrentContainer;
      pitem.ContainerId = pitem.Container.Id;

      _Service.AddItem(pitem);
      return pitem;
   }

   /// <summary>
   /// Update repository.
   /// </summary>
   /// <param name="pathItem"></param>
   /// <param name="updateParent"></param>
   /// <param name="updateItem"></param>
   private void UpdateRepository(CatalogPathItem pathItem, 
      bool updateParent, bool updateItem)
   {
      FileItemInfo item;
      if (updateParent)
      {
         item = CreateFileItem(
            pathItem.Parent.TreeItem, pathItem.Full);
      }
      if (updateItem)
      {
         pathItem.Item.Name = pathItem.NameFull;
         pathItem.Item.Description = pathItem.NameFull.
            Replace('.',' ').Replace('/',' ');
         //pathItem.Item.Container = _Service.CurrentContainer;
         pathItem.Item.ContainerId = _Service.CurrentContainer.Id;
         pathItem.Item.FullPath = pathItem.Full;
         pathItem.Item.ItemType = 
            pathItem.MediaFormat == DataObjects.Medias.MediaFormat.Unknown ?
               TreeItemType.Branch : TreeItemType.Leaf;

         pathItem.TreeItem.Title = pathItem.Item.Description;

         _Service.AddItem(pathItem.Item);
      }
   }

   /// <summary>
   /// Register leaf parent branch full path.
   /// </summary>
   /// <param name="fullPath">full path to register</param>
   public CatalogPathItem RegisterBranchPath(string fullPath)
   {
      //if (fullPath == null || fullPath[0] != '/')
      //{
      //   throw new ArgumentException(
      //      "CatalogTreeBuilder: a path must start with a slash.");
      //}

      var l = fullPath.Split('/');
      string path = String.Empty;
      CatalogPathItem parent = null;

      // must always start at 1 since all entries should start with a '/'
      for (int i = 0; i < l.Length; i++)
      {
         if (l[i] == String.Empty)
         {
            continue;
         }

         string name = l[i].Trim();
         path += "/" + name;
         if (path == String.Empty || path == "/")
         {
            continue;
         }
         
         // add branch to repository
         FileItemInfo item = _Service.CreateBranch(name);
         item.FullPath = path;
         item = _Service.AddItem(item);

         // add branch to registry
         var pathItem = new CatalogPathItem(item);
         if (!_Dictionary.TryGetValue(path, out CatalogPathItem value))
         {
            _Dictionary.TryAdd(path, pathItem);

            // setup tree item as needed
            if (pathItem.TreeItem == null)
            {
               pathItem.TreeItem = CreateItem(pathItem);
               pathItem.TreeItem.Number = ++_ItemCount;
            }
         }
         else
         {
            pathItem = value;
         }

         // finally add child to parent as needed
         if (parent == null && !_CatalogInfo.RootItem.Children.TryGetValue(
            pathItem.TreeItem, out CatalogItemInfo rootChild))
         {
            // add item to the root node
            _CatalogInfo.RootItem.Children.Add(pathItem.TreeItem);
         }
         else if (parent != null && !parent.TreeItem.Children.TryGetValue(
            pathItem.TreeItem, out CatalogItemInfo child))
         {
            parent.TreeItem.Children.Add(pathItem.TreeItem);
         }

         // setup next parent
         parent = pathItem;
      }

      return parent;
   }

   /// <summary>
   /// Get Catalog Item information from a File Item.
   /// </summary>
   /// <param name="pathItem">file item</param>
   /// <returns>instance of catalog item is returned</returns>
   public CatalogItemInfo GetItem(CatalogPathItem pathItem)
   {
      bool updateParent = false;
      bool updateItem = false;

      // try to find, if not found try to add paths
      if (!_Dictionary.TryGetValue(pathItem.Full, out CatalogPathItem item))
      {
         // add full path for new item
         _Dictionary.TryAdd(pathItem.Full, pathItem);
         updateItem = true;
         item = pathItem;
      }

      // setup parent as needed
      if (item.Parent == null)
      {
         item.Parent = RegisterBranchPath(pathItem.Path);
      }

      // is this the root path? if so, skip it...
      if (pathItem.Path == _CatalogInfo.RootFileItem.FullPath)
      {
         return _CatalogInfo.RootItem;
      }

      // setup tree item as needed
      if (item.TreeItem == null)
      {
         item.TreeItem = CreateItem(pathItem);
         item.TreeItem.Number = ++_ItemCount;
      }

      // finally add child to parent as needed
      if (pathItem.TreeItem != null &&
         !item.Parent.TreeItem.Children.TryGetValue(
         pathItem.TreeItem, out CatalogItemInfo child))
      {
         item.Parent.TreeItem.Children.Add(pathItem.TreeItem);
      }

      // item.Parent.TreeItem.Children.Add(item.TreeItem);

      // update repository
      UpdateRepository(pathItem, updateParent, updateItem);

      return item.TreeItem;
   }

   /// <summary>
   /// Get Catalog Item information from a File Item.
   /// </summary>
   /// <param name="item">file item</param>
   /// <returns>instance of catalog item is returned</returns>
   public CatalogPathItem GetItem(FileItemInfo item)
   {
      var pitem = new CatalogPathItem(item);
      var citem = GetItem(pitem);
      return pitem;
   }

   /// <summary>
   /// Get Catalog Item information based on given full path name.
   /// </summary>
   /// <param name="fullPath">full path</param>
   /// <returns>instance of catalog item is returned</returns>
   public CatalogPathItem GetItem(string fullPath)
   {
      FileItemInfo fitem = new FileItemInfo();
      fitem.FullPath = fullPath;
      CatalogPathItem item = new CatalogPathItem(fitem);
      var citem = GetItem(item);
      return item;
   }

   /// <summary>
   /// Convert to a Catalog Path Item.
   /// </summary>
   /// <param name="item">file item to convert</param>
   /// <returns>instance of CatalogItemInfo is returned</returns>
   public static CatalogPathItem ToPathItem(FileItemInfo item)
   {
      CatalogPathItem pitem = new CatalogPathItem(item);
      pitem.TreeItem = CreateItem(pitem);
      return pitem;
   }

   /// <summary>
   /// Given a path, clean it up and return a valid path.
   /// </summary>
   /// <param name="path">path to review</param>
   /// <returns>clen-up path is returned</returns>
   public string GetPath(string? path)
   {
      // is this an empty path? if so, set it up as the root path
      string spath = String.IsNullOrWhiteSpace(path) ? "/" : path;
      var lastChar = spath.Length > 1 ? spath[spath.Length - 1] : ' ';
      if (lastChar == '/')
      {
         spath = spath.Substring(0, spath.Length - 1);
      }

      // does we have a driver of sort specified?
      int indx = spath.IndexOf('/');
      if (indx >= 0)
      {
         spath = spath.Substring(indx);
      }

      return spath;
   }

   /// <summary>
   /// Get branch items for current container.
   /// </summary>
   /// <param name="path">path to branch</param>
   public void GetBranch(string? path = null)
   {
      string spath = GetPath(path);
      spath = spath.Length > 1 ? spath + "/" : spath;
      var items = _Service.GetBranch(spath);
      foreach (var item in items)
      {
         GetItem(item);
      }
   }

   /// <summary>
   /// Build the tree based on the catalog items path
   /// </summary>
   /// <param name="resetCatalog">reset catalog [default: true]</param>
   /// <returns>instance of Catalog is returned</returns>
   public void BuildTree(bool resetCatalog = true)
   {
      if (resetCatalog)
      {
         _Dictionary = _CatalogInfo.CatalogDictionary;
         _Dictionary.Clear();
      }

      GetBranch();
   }

}