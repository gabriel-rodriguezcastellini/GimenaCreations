using GimenaCreations.Constants;
using GimenaCreations.Data;
using GimenaCreations.Entities;
using GimenaCreations.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public List<Sale> Sales { get; set; } = new();

        [BindProperty]
        public SalesPerformance SalePerformance { get; set; }

        [BindProperty]
        public int NewClients { get; set; }

        [BindProperty]
        public decimal TotalSales { get; set; }

        [BindProperty]
        public decimal TotalCost { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Admin.View)).Succeeded)
            {
                return new ForbidResult();
            }

            foreach (var item in (await _context.OrderItems.Select(x => new { x.CatalogItemId, x.Total, x.ProductName }).AsNoTracking().ToListAsync()).GroupBy(x => x.CatalogItemId))
            {
                Sales.Add(new()
                {
                    ProductName = item.First().ProductName,
                    Total = Math.Round(item.Sum(x => x.Total), 2)
                });
            }

            var startOfTthisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var firstDay = startOfTthisMonth.AddMonths(-1);
            var lastDay = startOfTthisMonth.AddDays(-1);
            SalePerformance = new();

            var lastMonthUniquePurchases = (await _context.Orders.Where(x => x.Date >= firstDay && x.Date <= lastDay && x.Status == OrderStatus.Paid)
                .Select(x => new { x.ApplicationUserId })
                .AsNoTracking()
                .ToListAsync())
                .GroupBy(x => x.ApplicationUserId)
                .Count();

            var uniquePurchases = (await _context.Orders.Where(x => x.Date >= startOfTthisMonth && x.Status == OrderStatus.Paid).Select(x => new { x.ApplicationUserId }).AsNoTracking()
                .ToListAsync())
                .GroupBy(x => x.ApplicationUserId)
                .Count();

            SalePerformance.UniquePurchase.Value = uniquePurchases;
            SalePerformance.UniquePurchase.AbsoluteChange = Math.Abs(uniquePurchases - lastMonthUniquePurchases);
            SalePerformance.UniquePurchase.Sign = uniquePurchases >= lastMonthUniquePurchases ? Sign.Positive : Sign.Negative;

            SalePerformance.UniquePurchase.PercentageChange = uniquePurchases >= lastMonthUniquePurchases
                ? Math.Abs(lastMonthUniquePurchases * 100 / uniquePurchases - 100)
                : uniquePurchases * 100 / lastMonthUniquePurchases - 100;

            var purchases = (await _context.Orders.Where(x => x.Date >= startOfTthisMonth && x.Status == OrderStatus.Paid).AsNoTracking().ToListAsync()).Count;
            var lastMonthPurchases = (await _context.Orders.Where(x => x.Date >= firstDay && x.Date <= lastDay && x.Status == OrderStatus.Paid).AsNoTracking().ToListAsync()).Count;
            SalePerformance.Quantity.Value = purchases;
            SalePerformance.Quantity.Sign = purchases >= lastMonthPurchases ? Sign.Positive : Sign.Negative;
            SalePerformance.Quantity.AbsoluteChange = Math.Abs(purchases - lastMonthPurchases);

            SalePerformance.Quantity.PercentageChange = purchases >= lastMonthPurchases ? Math.Abs(lastMonthUniquePurchases * 100 / purchases - 100) :
                purchases * 100 / lastMonthPurchases - 100;

            var productRevenue = (await _context.Orders.Include(x => x.Items).Where(x => x.Status == OrderStatus.Paid && x.Date >= startOfTthisMonth).AsNoTracking().ToListAsync())
                .Sum(x => x.GetTotal()) - (await _context.Purchases.Include(x => x.Items).Where(x => x.PurchaseDate >= startOfTthisMonth).AsNoTracking().ToListAsync())
                .Sum(x => x.GetTotal());

            var lastMonthProductRevenue = (await _context.Orders.Include(x => x.Items).Where(x => x.Status == OrderStatus.Paid && x.Date >= firstDay && x.Date <= lastDay).AsNoTracking()
                .ToListAsync())
                .Sum(x => x.GetTotal()) - (await _context.Purchases.Include(x => x.Items).Where(x => x.PurchaseDate >= firstDay && x.PurchaseDate <= lastDay).AsNoTracking()
                .ToListAsync())
                .Sum(x => x.GetTotal());

            SalePerformance.ProductRevenue.Sign = productRevenue >= lastMonthProductRevenue ? Sign.Positive : Sign.Negative;
            SalePerformance.ProductRevenue.Value = productRevenue;
            SalePerformance.ProductRevenue.AbsoluteChange = Math.Abs(productRevenue - lastMonthProductRevenue);

            SalePerformance.ProductRevenue.PercentageChange = productRevenue >= lastMonthProductRevenue ? Math.Abs(lastMonthProductRevenue * 100 / productRevenue - 100)
                : productRevenue * 100 / lastMonthProductRevenue - 100;

            foreach (var item in await _context.Users.Where(x => x.DateTimeAdd >= startOfTthisMonth).ToListAsync())
            {
                NewClients++;
            }

            TotalSales = (await _context.Orders.Where(x => x.Date >= startOfTthisMonth && x.Status == OrderStatus.Paid).Include(x => x.Items).Select(x => x.GetTotal()).ToListAsync())
                .Sum(x => x);

            TotalCost = (await _context.Purchases.Where(x => x.PurchaseDate >= startOfTthisMonth).Include(x => x.Items).Select(x => x.GetTotal()).ToListAsync())
                .Sum(x => x);

            return Page();
        }
    }
}
