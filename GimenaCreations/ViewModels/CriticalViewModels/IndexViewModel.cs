using GimenaCreations.Entities;

namespace GimenaCreations.ViewModels.CriticalViewModels;

public class CriticalStockViewModel
{
    public List<CatalogItem> CatalogItems { get; set; }
    public string Disabled => !CatalogItems.Any() ? "disabled" : "";
}
