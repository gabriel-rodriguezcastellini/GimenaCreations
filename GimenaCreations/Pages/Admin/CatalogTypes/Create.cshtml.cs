using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogTypes
{
    public class CreateModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            return Page();
        }

        [BindProperty]
        public CatalogType CatalogType { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CatalogTypes.Add(CatalogType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
