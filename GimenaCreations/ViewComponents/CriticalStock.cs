using GimenaCreations.Constants;
using GimenaCreations.Services;
using GimenaCreations.ViewModels.CriticalViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GimenaCreations.ViewComponents;

[Authorize(Permissions.CriticalStock.View)]
public class CriticalStock : ViewComponent
{
    private readonly ICatalogService _catalogService;

    public CriticalStock(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IViewComponentResult> InvokeAsync() => View(new CriticalStockViewModel
    {
        CatalogItems = await _catalogService.GetItemsWithCriticalStockAsync()
    });
}
