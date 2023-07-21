using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using GimenaCreations.PDF.Presentation;
using GimenaCreations.Extensions;
using QuestPDF.Fluent;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Purchases
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

        public IList<Purchase> Purchase { get; set; } = default!;

        [BindProperty]
        [Display(Name = "Start date")]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.View)).Succeeded)
            {
                return new ForbidResult();
            }

            IQueryable<Purchase> purchases = null;

            if (_context.Purchases != null)
            {
                purchases = _context.Purchases.Include(p => p.Items)
                .Include(p => p.ApplicationUser)
                .Include(p => p.Supplier);
            }

            if (startDate != null)
            {
                purchases = purchases.Where(x => x.PurchaseDate >= startDate);
            }

            if (endDate != null)
            {
                purchases = purchases.Where(x => x.PurchaseDate <= endDate);
            }

            Purchase = await purchases.OrderByDescending(x => x.Id).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetReportAsync(int purchaseId)
        {
            var model = await _context.Purchases.Include(x => x.Items).ThenInclude(x => x.CatalogItem).Include(x => x.ApplicationUser).FirstAsync(x => x.Id == purchaseId);

            var document = new PurchaseDocument(new PDF.DocumentModels.PurchaseModel
            {
                Date = model.PurchaseDate,
                Id = purchaseId,
                Importance = model.Importance.GetDisplayName(),
                PaymentMethod = model.PaymentMethod.GetDisplayName(),
                RequestedBy = model.ApplicationUser.FullName,
                Items = model.Items.Select(x => new PDF.DocumentModels.PurchaseItem
                {
                    CatalogItemId = x.CatalogItemId,
                    Price = x.Price,
                    ProductName = x.CatalogItem.Name,
                    Quantity = x.Quantity
                }).ToList()
            });

            document.GeneratePdf("purchase.pdf");
            Process.Start("explorer.exe", "purchase.pdf");
            return RedirectToPage("/Admin/Purchases/Index");
        }

        public async Task<IActionResult> OnPostReportListAsync(DateTime? startDate, DateTime? endDate)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.View)).Succeeded)
            {
                return new ForbidResult();
            }

            var model = _context.Purchases.Include(x => x.ApplicationUser).AsQueryable();

            if (startDate != null)
            {
                model = model.Where(x => x.PurchaseDate >= startDate);
            }

            if (endDate != null)
            {
                model = model.Where(x => x.PurchaseDate <= endDate);
            }

            var data = await model.ToListAsync();

            var document = new PurchaseListDocument(data.Select(x => new PDF.DocumentModels.PurchaseModel
            {
                Date = x.PurchaseDate,
                Id = x.Id,
                Importance = x.Importance.GetDisplayName(),
                PaymentMethod = x.PaymentMethod.GetDisplayName(),
                RequestedBy = x.ApplicationUser.FullName
            }).ToList());

            document.GeneratePdf("purchases.pdf");
            Process.Start("explorer.exe", "purchases.pdf");
            return RedirectToPage("/Admin/Purchases/Index");
        }
    }
}
