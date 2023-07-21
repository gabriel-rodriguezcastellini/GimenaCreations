using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class PurchaseItem
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Display(Name = "Catalog item")]
    public int CatalogItemId { get; set; }

    public CatalogItem CatalogItem { get; set; }
    public decimal Price { get; set; }
    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; }
}
