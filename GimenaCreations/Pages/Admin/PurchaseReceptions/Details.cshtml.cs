using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.PurchaseReceptions
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

        public PurchaseReception PurchaseReception { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.PurchaseReceptions == null)
            {
                return NotFound();
            }

            var purchasereception = await _context.PurchaseReceptions.FirstOrDefaultAsync(m => m.Id == id);
            if (purchasereception == null)
            {
                return NotFound();
            }
            else 
            {
                PurchaseReception = purchasereception;
            }
            return Page();
        }
    }
}
