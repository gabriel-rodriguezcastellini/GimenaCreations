using GimenaCreations.Entities;
using GimenaCreations.Models;

namespace GimenaCreations.Services;

public interface ICartService
{
    Task<CustomerBasket> GetBasketAsync(string userId);
    Task AddItemToBasketAsync(string userId, int productId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task<Order> CheckoutAsync(CustomerBasket checkout, string userId);
    Task<CustomerBasket> SetQuantitiesAsync(string userId, Dictionary<string, int> quantities);
    Task DeleteItemAsync(string basketId, int productId);
    Task DeleteBasketAsync(string id);
}
