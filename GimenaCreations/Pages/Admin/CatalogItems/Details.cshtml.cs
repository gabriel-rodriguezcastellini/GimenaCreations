using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogItems
{
    public class DetailsModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DetailsModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public CatalogItem CatalogItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogItems == null)
            {
                return NotFound();
            }

            var catalogitem = await _context.CatalogItems.Include(x => x.CatalogType).FirstOrDefaultAsync(m => m.Id == id);
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
