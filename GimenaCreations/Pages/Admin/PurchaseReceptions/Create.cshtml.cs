using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.PurchaseReceptions
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
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "Id", "Id");            
            return Page();
        }

        [BindProperty]
        public PurchaseReception PurchaseReception { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _context.PurchaseReceptions.Add(PurchaseReception);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
