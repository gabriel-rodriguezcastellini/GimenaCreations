using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Orders
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

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Orders.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(x => x.ApplicationUser).Include(x => x.Items).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}
