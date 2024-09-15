using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.Data.CatalogModel;

namespace Edam.UI.Catalog.Controls;

public class CatalogContainerViewModel : ObservableObject
{

    public ObservableCollection<ContainerInfo> DataSource { get; set; } =
        new ObservableCollection<ContainerInfo>();

    public CatalogContainerViewModel()
    {

    }

}



