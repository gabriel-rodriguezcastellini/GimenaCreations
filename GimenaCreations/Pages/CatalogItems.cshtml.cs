using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimenaCreations.Services;
using GimenaCreations.Entities;

namespace GimenaCreations.Pages
{
    public class CatalogItemsModel : PageModel
    {        
        private readonly ICatalogService _catalogService;

        public CatalogItemsModel(ICatalogService catalogService)
        {            
            _catalogService = catalogService;
        }

        public CatalogItem CatalogItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {            
            var catalogitem = await _catalogService.GetCatalogItemAsync(id);

            if (catalogitem == null)
            {
                return NotFound();
            }
            else
            {
                CatalogItem = catalogitem;
            }

            return Page();
        }
    }
}
