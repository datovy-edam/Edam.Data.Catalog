using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

using Edam.Data.CatalogModel;
using Edam.Diagnostics;
using Edam.UI.Catalog.Models;

namespace Edam.UI.Catalog.Controls;

public class CatalogContainerViewModel : ObservableObject
{

    public CatalogViewModel CatalogBase { get; set; }

    private Visibility _containerEditVisibility;
    public Visibility ContainerEditVisibility
    {
        get { return _containerEditVisibility; }
        set
        {
            if (_containerEditVisibility != value)
            {
                _containerEditVisibility = value;
                OnPropertyChanged(nameof(ContainerEditVisibility));
            }
        }
    }

    public ObservableCollection<ContainerItem> DataSource { get; set; } =
        new ObservableCollection<ContainerItem>();

    public CatalogContainerViewModel()
    {
        ContainerEditVisibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Initialize Catalog
    /// </summary>
    public async Task InitializeContainersAsync()
    {
        DataSource.Clear();
        var lst = await CatalogBase.Catalog.CatalogService.GetContainersAsync();

        foreach(var item in lst)
        {
            var container = new ContainerItem();
            container.Container = item;
            DataSource.Add(container);
        }
    }

}



