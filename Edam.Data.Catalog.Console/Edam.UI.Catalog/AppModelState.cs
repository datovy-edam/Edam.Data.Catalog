using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

// -----------------------------------------------------------------------------

namespace Edam.UI.Catalog;
public class AppModelState
{
    public IStringLocalizer Localizer { get; set; }
    public IOptions<AppConfig> AppOptions { get; set; }
    public INavigator Navigator { get; set; }

    public string? GetDefaultConnectionString()
    {
        return AppOptions?.Value?.DefaultConnectionString;
    }
}