using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GimenaCreations.Pages.Admin.CatalogTypes
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public IList<CatalogType> CatalogType { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.CatalogTypes != null)
            {
                CatalogType = await _context.CatalogTypes.ToListAsync();
            }

            return Page();
        }
    }
}
