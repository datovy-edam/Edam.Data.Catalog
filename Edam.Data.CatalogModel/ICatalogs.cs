using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Diagnostics;

namespace Edam.Data.FileSystemModel;

public interface ICatalogs
{
   string GetCurrentCatalogName();
   string GetDefaultCatalogName();

   // instantiate a named catalog service
   ResultsLog<ICatalogService> GetCatalog(string invariantName);
}
