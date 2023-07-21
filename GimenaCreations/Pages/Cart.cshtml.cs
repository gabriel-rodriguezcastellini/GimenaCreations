using GimenaCreations.Entities;
using GimenaCreations.Models;
using GimenaCreations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartModel(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public CustomerBasket CustomerBasket { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CustomerBasket = await _cartService.GetBasketAsync(_userManager.GetUserId(HttpContext.User));

            if (CustomerBasket == null || !CustomerBasket.Items.Any())
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string currentFilter, int? catalogTypeId, int? pageIndex, int catalogItemId, string returnUrl)
        {
            await _cartService.AddItemToBasketAsync(_userManager.GetUserId(HttpContext.User), catalogItemId);

            return returnUrl == "CatalogItems"
                ? RedirectToPage(returnUrl, new { id = catalogItemId })
                : (IActionResult)RedirectToPage(returnUrl, new { currentFilter, catalogTypeId, pageIndex });
        }

        public async Task<IActionResult> OnPostDeleteItemAsync(int productId)
        {
            await _cartService.DeleteItemAsync(_userManager.GetUserId(HttpContext.User), productId);
            return RedirectToPage("Cart");
        }

        public async Task<IActionResult> OnPostUpdateAsync(Dictionary<string, int> quantities, string action)
        {
            await _cartService.SetQuantitiesAsync(_userManager.GetUserId(HttpContext.User), quantities);

            if (action == "CHECKOUT")
            {
                return RedirectToPage("Order");
            }

            return RedirectToPage("Cart");
        }
    }
}
