namespace Edam.UI.Catalog;

public record AppConfig
{
    public string? Environment { get; init; }
    public string? DefaultConnectionString { get; init; }
    public string? CatalogServiceBaseUri { get; init; }
}
