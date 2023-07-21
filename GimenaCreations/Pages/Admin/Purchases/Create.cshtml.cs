using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GimenaCreations.Entities;
using System.Security.Claims;
using GimenaCreations.Extensions;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Purchases
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
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "BusinessName");
            ViewData["Importance"] = new SelectList(Enum.GetValues<Importance>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name");
            ViewData["PaymentMethod"] = new SelectList(Enum.GetValues<PaymentMethod>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name");
            return Page();
        }

        [BindProperty]
        public Purchase Purchase { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Purchase.PurchaseStatus = PurchaseStatus.Submitted;
            Purchase.ApplicationUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await _context.Purchases.AddAsync(Purchase);
            await _context.SaveChangesAsync();

            await _context.PurchaseHistories.AddAsync(new PurchaseHistory
            {
                PurchaseId = Purchase.Id,
                Items = new List<PurchaseHistoryItem>
                {
                    new()
                    {
                        Date = DateTime.UtcNow,
                        State = Purchase.PurchaseStatus.GetDisplayName()
                    }
                }
            });

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
