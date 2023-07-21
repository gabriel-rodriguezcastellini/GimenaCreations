using GimenaCreations.Entities;
using GimenaCreations.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace GimenaCreations.Services;

public class CartService : ICartService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ICatalogService _catalogService;
    private readonly ILogger<CartService> _logger;

    public CartService(IConnectionMultiplexer redis, ICatalogService catalogService, ILogger<CartService> logger)
    {
        _redis = redis;
        _catalogService = catalogService;
        _logger = logger;
    }

    public async Task AddItemToBasketAsync(string userId, int productId)
    {
        var item = await _catalogService.GetCatalogItemAsync(productId);
        var basket = await GetBasketAsync(userId);
        var product = basket.Items.SingleOrDefault(i => i.ProductId == productId);

        if (product == null)
        {
            basket.Items.Add(new BasketItem
            {
                ProductId = productId,
                UnitPrice = item!.Price,
                PictureUrl = item.PictureFileName,
                ProductName = item.Name,
                Quantity = 1,
                RequiresFile = item.RequiresFile
            });
        }
        else
        {
            product.Quantity++;
        }

        await UpdateBasketAsync(basket);
    }

    public async Task<Entities.Order> CheckoutAsync(CustomerBasket checkout, string userId)
    {
        var order = new Entities.Order
        {
            Address = new()
            {
                City = checkout.User.Address.City,
                Country = checkout.User.Address.Country,
                State = checkout.User.Address.State,
                Street = checkout.User.Address.Street,
                ZipCode = checkout.User.Address.ZipCode
            },
            Items = (await GetBasketAsync(userId)).Items.Select(i => new OrderItem
            {
                CatalogItemId = i.ProductId,
                PictureUrl = i.PictureUrl,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Units = i.Quantity,
                RequiresFile = i.RequiresFile
            }).ToList(),
            Date = DateTime.UtcNow,
            PaymentMethod = (PaymentMethod)checkout.PaymentMethod!,
            ApplicationUserId = userId,
            Status = OrderStatus.Submited
        };

        await DeleteBasketAsync(userId);
        return order;
    }

    public async Task DeleteBasketAsync(string id)
    {
        await _redis.GetDatabase().KeyDeleteAsync(id);
    }

    public async Task DeleteItemAsync(string basketId, int productId)
    {
        var basket = await _redis.GetDatabase().StringGetAsync(basketId);

        if (basket.IsNullOrEmpty)
        {
            _logger.LogInformation("Problem occur deleting the item.");
        }

        var basketDto = JsonSerializer.Deserialize<CustomerBasket>(basket!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        basketDto!.Items.Remove(basketDto.Items.First(x => x.ProductId == productId));
        await UpdateBasketAsync(basketDto);
    }

    public async Task<CustomerBasket> GetBasketAsync(string userId)
    {
        var basket = await _redis.GetDatabase().StringGetAsync(userId);

        if (basket.IsNullOrEmpty)
        {
            return new CustomerBasket(userId);
        }

        return JsonSerializer.Deserialize<CustomerBasket>(basket!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<CustomerBasket> SetQuantitiesAsync(string userId, Dictionary<string, int> quantities)
    {
        var basket = await GetBasketAsync(userId);

        foreach (var item in basket.Items)
        {
            item.Quantity = quantities.First(x => x.Key == item.ProductId.ToString()).Value;
        }

        return await UpdateBasketAsync(basket);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await _redis.GetDatabase().StringSetAsync(basket.User.Id, JsonSerializer.Serialize(basket));

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return null;
        }

        _logger.LogInformation("Basket item persisted succesfully.");
        return await GetBasketAsync(basket.User.Id);
    }
}
