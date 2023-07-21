using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Purchases
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
        public Purchase Purchase { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.Include(x => x.ApplicationUser).FirstOrDefaultAsync(m => m.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }
            else
            {
                Purchase = purchase;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase != null)
            {
                Purchase = purchase;
                _context.Purchases.Remove(Purchase);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
