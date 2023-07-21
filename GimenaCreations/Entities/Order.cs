using GimenaCreations.MarkerInterfaces;
using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class Order : IAuditable
{
    public int Id { get; set; }
    public Address Address { get; set; }
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; }

    [Display(Name = "Payment method")]
    public PaymentMethod PaymentMethod { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; }

    [Display(Name = "User")]
    public ApplicationUser ApplicationUser { get; set; }

    public string ApplicationUserId { get; set; }

    public decimal GetTotal() => Items.Sum(o => o.Units * o.UnitPrice);
}
