using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.PurchasesChangeHistory
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

        public IList<PurchaseHistory> PurchaseHistory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchasesChangeHistory.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.PurchaseHistories != null)
            {
                PurchaseHistory = await _context.PurchaseHistories.AsNoTracking().Include(x => x.Items).Include(x => x.Purchase).ThenInclude(x => x.Supplier).OrderByDescending(x => x.Id)
                    .ToListAsync();
            }

            return Page();
        }
    }
}
