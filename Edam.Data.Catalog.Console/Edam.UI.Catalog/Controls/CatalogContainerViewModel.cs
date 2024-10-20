using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

using Edam.Data.CatalogModel;
using Edam.Diagnostics;

namespace Edam.UI.Catalog.Controls;

public class CatalogContainerViewModel : ObservableObject
{

    public CatalogViewModel CatalogBase { get; set; }

    public ObservableCollection<ContainerInfo> DataSource { get; set; } =
        new ObservableCollection<ContainerInfo>();

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async Task InitializeContainersAsync()
    {
        DataSource.Clear();
        var list = await CatalogBase.Catalog.
            CatalogService.GetContainersAsync();

        foreach(var item in list)
        {
            DataSource.Add(item);
        }
    }

}



