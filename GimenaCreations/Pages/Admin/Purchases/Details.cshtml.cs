using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Purchases
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

        public Purchase Purchase { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.View)).Succeeded)
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
    }
}
