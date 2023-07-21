using GimenaCreations.Entities;
using GimenaCreations.Services;
using GimenaCreations.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GimenaCreations.ViewComponents;

public class Cart : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICartService _cartService;

    public Cart(UserManager<ApplicationUser> userManager, ICartService cartService)
    {
        _userManager = userManager;
        _cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync() => View(new CartComponentViewModel() 
    { 
        ItemsCount = (await _cartService.GetBasketAsync(_userManager.GetUserId(HttpContext.User))).Items.Count 
    });
}
