using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Suppliers
{
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Supplier Supplier { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }
            else
            {
                Supplier = supplier;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier != null)
            {
                Supplier = supplier;
                _context.Suppliers.Remove(Supplier);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
