﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Data.FileSystemModel;
using Edam.DataObjects.Entities;
using Edam.Diagnostics;

namespace Edam.Data.FileSystemDb;

public class CatalogInstance : ICatalogs
{
   public const string EDAM_FILE_SYSTEM_DB = "edam.file.system.db";

   private static string _CatalogName = EDAM_FILE_SYSTEM_DB;

   public string GetCurrentCatalogName()
   {
      return _CatalogName;
   }

   public string GetDefaultCatalogName()
   {
      return EDAM_FILE_SYSTEM_DB;
   }

   /// <summary>
   /// Get an instance of a Catalog Service by name.
   /// </summary>
   /// <param name="invariantName">name of the catalog instance</param>
   /// <returns>an instance of the requested catalog is returned</returns>
   public ResultsLog<ICatalogService?> GetCatalog(string invariantName)
   {
      ResultsLog<ICatalogService?> results = new ResultsLog<ICatalogService?>();
      if (!String.IsNullOrWhiteSpace(invariantName))
      {
         switch(invariantName)
         {
            case EDAM_FILE_SYSTEM_DB:
               _CatalogName = EDAM_FILE_SYSTEM_DB;
               results.Instance = new FileSystemInstance();
               results.Succeeded();
               break;
            default:
               results.Failed(EventCode.NotSupported);
               break;
         }
      }
      else
      {
         results.Failed(EventCode.ArgumentOrParameterExpectedNotFound);
      }
      return results;
   }
}