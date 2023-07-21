using GimenaCreations.Entities;
using GimenaCreations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GimenaCreations.Pages;

[Authorize]
public class OrderDetailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOrderService _orderService;
    private readonly IInvoiceService _invoiceService;

    public OrderDetailModel(UserManager<ApplicationUser> userManager, IOrderService orderService, IInvoiceService invoiceService)
    {
        _userManager = userManager;
        _orderService = orderService;
        _invoiceService = invoiceService;
    }

    [BindProperty]
    public Order Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int orderId)
    {
        Order = await _orderService.GetOrderByIdAsync(orderId, _userManager.GetUserId(HttpContext.User));
        return Page();
    }

    public async Task<IActionResult> OnGetInvoiceAsync(int orderId)
    {
        await _invoiceService.GenerateInvoiceAsync(orderId, User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        return RedirectToPage("OrderDetail", new { orderId });
    }
}
