using GimenaCreations.Entities;
using GimenaCreations.Pagination;
using GimenaCreations.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogService _catalogService;

        public IndexModel(ICatalogService catalogService)
        {            
            _catalogService = catalogService;
        }

        public PaginatedList<CatalogItem> CatalogItem { get; set; } = default!;
        public IList<CatalogType> CatalogType { get; set; } = default!;
        public string CurrentFilter { get; set; }
        public int? CatalogTypeId { get; set; }
        public int? CatalogItemId { get; set; }

        public async Task OnGetAsync(string searchString, string currentFilter, int? catalogTypeId, int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;
            CatalogTypeId = catalogTypeId;
            CatalogItem = await _catalogService.GetCatalogItemsAsync(searchString, catalogTypeId, pageIndex);
            CatalogType = await _catalogService.GetCatalogTypesAsync();
        }
    }
}