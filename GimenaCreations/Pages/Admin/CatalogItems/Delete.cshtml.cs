using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogItems
{
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(Data.ApplicationDbContext context, IWebHostEnvironment environment, IAuthorizationService authorizationService)
        {
            _context = context;
            _environment = environment;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public CatalogItem CatalogItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Delete)).Succeeded)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogItems == null)
            {
                return NotFound();
            }
            var catalogitem = await _context.CatalogItems.FindAsync(id);

            if (catalogitem != null)
            {
                System.IO.File.Delete($"{_environment.WebRootPath}\\{CatalogItem.PictureFileName}");
                CatalogItem = catalogitem;
                _context.CatalogItems.Remove(CatalogItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
