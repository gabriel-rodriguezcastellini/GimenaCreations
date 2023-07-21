using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.PurchaseReceptions
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
        public PurchaseReception PurchaseReception { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.Edit)).Succeeded)
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
            PurchaseReception = purchasereception;
            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "Id", "Id");            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PurchaseReception).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseReceptionExists(PurchaseReception.Id))
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

        private bool PurchaseReceptionExists(int id)
        {
            return _context.PurchaseReceptions.Any(e => e.Id == id);
        }
    }
}
