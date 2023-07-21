using GimenaCreations.Entities;
using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Models;

public class CustomerBasket
{
    public CustomerBasket()
    {

    }

    public CustomerBasket(string customerId) => User.Id = customerId;
    public ApplicationUser User { get; set; } = new() { Address = new() };
    public List<BasketItem> Items { get; set; } = new();

    [Required]
    public PaymentMethod? PaymentMethod { get; set; }

    public decimal Total() => Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
}
