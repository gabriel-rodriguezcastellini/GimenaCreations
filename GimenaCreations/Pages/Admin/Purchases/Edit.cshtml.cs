using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using GimenaCreations.Extensions;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Purchases
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
        public Purchase Purchase { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Edit)).Succeeded)
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
            Purchase = purchase;
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "BusinessName");
            ViewData["Importance"] = new SelectList(Enum.GetValues<Importance>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name");
            ViewData["PaymentMethod"] = new SelectList(Enum.GetValues<PaymentMethod>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name");
            ViewData["Status"] = new SelectList(Enum.GetValues<PurchaseStatus>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(Purchase.Id))
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

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }
    }
}
