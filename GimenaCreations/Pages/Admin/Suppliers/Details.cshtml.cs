using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Suppliers
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

        public Supplier Supplier { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.View)).Succeeded)
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
    }
}
