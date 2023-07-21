using GimenaCreations.MarkerInterfaces;
using Microsoft.EntityFrameworkCore.Update;
using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class Purchase : IAuditable, IUpdateable
{
    public int Id { get; set; }

    public Supplier Supplier { get; set; }

    [Display(Name = "Supplier")]
    public int SupplierId { get; set; }

    [Display(Name = "Purchase order date")]
    public DateTime PurchaseDate { get; set; }

    public Importance Importance { get; set; }

    [Display(Name = "Requested by")]
    public ApplicationUser ApplicationUser { get; set; }

    [Display(Name = "Requested by")]
    public string ApplicationUserId { get; set; }

    [Display(Name = "Payment method")]
    public PaymentMethod PaymentMethod { get; set; }

    [Display(Name = "Payment deadline")]
    public DateTime PaymentDeadline { get; set; }

    public DateTime Deadline { get; set; }

    [Display(Name = "Recipient name")]
    [Required]
    public string RecipientName { get; set; }

    [Display(Name = "Shipping company")]
    public string ShippingCompany { get; set; }

    [Display(Name = "Shipping address")]
    [Required]
    public string ShippingAddress { get; set; }

    [Display(Name = "Shipping city")]
    [Required]
    public string ShippingCity { get; set; }

    [Display(Name = "Phone")]
    [Phone]
    public string Phone { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Discount { get; set; }

    public string Comments { get; set; }

    [Display(Name = "Status")]
    public PurchaseStatus PurchaseStatus { get; set; }

    public List<PurchaseItem> Items { get; set; }

    public DateTime ModificationDate { get; set; }

    public decimal GetTotal() => Items.Sum(x => x.Quantity * x.Price);
}
