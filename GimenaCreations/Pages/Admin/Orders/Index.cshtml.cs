using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GimenaCreations.Pages.Admin.Orders
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

        public IList<Order> Order { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Orders.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.Orders != null)
            {
                Order = await _context.Orders
                .Include(o => o.ApplicationUser).Include(x=>x.Items).ThenInclude(x=>x.Files).OrderByDescending(x=>x.Id).ToListAsync();
            }

            return Page();
        }
    }
}
