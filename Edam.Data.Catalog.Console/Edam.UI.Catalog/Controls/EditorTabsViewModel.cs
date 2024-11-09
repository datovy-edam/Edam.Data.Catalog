using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edam.Data.CatalogModel;

namespace Edam.UI.Catalog.Controls;

public class EditorTabsViewModel
{
    public string DocumentName { get; set; }
    public CatalogPathItem CurrentPathItem { get; set; }
    public string CurrentLanguage { get; set; }
    public ObservableCollection<MonacoEditorViewModel> 
        EditorTabs { get; set; } = new 
           ObservableCollection<MonacoEditorViewModel>();
}
