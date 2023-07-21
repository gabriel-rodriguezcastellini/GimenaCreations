using GimenaCreations.Contracts;
using GimenaCreations.Entities;
using GimenaCreations.Models;
using GimenaCreations.Services;
using MassTransit;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages;

[Authorize]
public class OrderModel : PageModel
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly ICatalogService _catalogService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBus bus;
    private readonly IConfiguration _configuration;

    public OrderModel(ICartService cartService,
        UserManager<ApplicationUser> userManager,
        IOrderService orderService,
        ICatalogService catalogService,
        IBus bus,
        IConfiguration configuration)
    {
        _cartService = cartService;
        _userManager = userManager;
        _orderService = orderService;
        _catalogService = catalogService;
        this.bus = bus;
        _configuration = configuration;
    }

    [BindProperty]
    public CustomerBasket Checkout { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        Checkout = await _cartService.GetBasketAsync(user.Id);

        Checkout.User = new()
        {
            Address = new()
            {
                State = user.Address.State,
                City = user.Address.City,
                Country = user.Address.Country,
                Street = user.Address.Street,
                ZipCode = user.Address.ZipCode,
            },
            FirstName = user.FirstName,
            LastName = user.LastName,
        };        
    }

    /// <summary>
    /// Crea la orden, el stock debe ser actualizado sincrónicamente, porque si se hace asíncronicamente da error EF Core
    /// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#avoiding-dbcontext-threading-issues
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        var order = await _cartService.CheckoutAsync(Checkout, _userManager.GetUserId(HttpContext.User));
        await _orderService.CreateOrderAsync(order);
        order.Items.ToList().ForEach(x => _catalogService.UpdateCatalogItemStockAsync(x.CatalogItemId, -x.Units).Wait());
        _ = bus.Publish(new OrderStatusChangedToSubmitted(order.Id, order.Status, order.ApplicationUserId));

        if (order.PaymentMethod == PaymentMethod.MercadoPago)
        {
            MercadoPagoConfig.AccessToken = _configuration.GetValue<string>("MercadoPago:AccessToken");

            // Crea la preferencia
            var preference = await new PreferenceClient().CreateAsync(new PreferenceRequest
            {
                Items = order.Items
                .Select(x => new PreferenceItemRequest
                {
                    Title = x.ProductName,
                    Quantity = x.Units,
                    CurrencyId = "ARS",
                    UnitPrice = x.UnitPrice
                }).ToList(),
                Purpose = "wallet_purchase",
                ExternalReference = order.Id.ToString(),
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Failure = _configuration.GetValue<string>("MercadoPago:BackUrls:Failure"),
                    Pending = _configuration.GetValue<string>("MercadoPago:BackUrls:Pending"),
                    Success = _configuration.GetValue<string>("MercadoPago:BackUrls:Success")
                },
                NotificationUrl = $"{_configuration.GetValue<string>("MercadoPago:NotificationUrl")}?source_news={_configuration.GetValue<string>("MercadoPago:SourceNews")}",
                AutoReturn = "approved"
            });

            return RedirectPermanent(preference.InitPoint);
        }

        return RedirectToPage("OrderManagement");
    }
}
