using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogTypes
{
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public CatalogType CatalogType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogTypes == null)
            {
                return NotFound();
            }

            var catalogtype =  await _context.CatalogTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (catalogtype == null)
            {
                return NotFound();
            }
            CatalogType = catalogtype;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CatalogType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogTypeExists(CatalogType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CatalogTypeExists(int id)
        {
          return _context.CatalogTypes.Any(e => e.Id == id);
        }
    }
}
