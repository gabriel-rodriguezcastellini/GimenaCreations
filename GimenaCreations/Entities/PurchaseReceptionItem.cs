using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class PurchaseReceptionItem
{
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Display(Name = "Catalog item")]
    public CatalogItem CatalogItem { get; set; }

    [Display(Name = "Catalog item ID")]
    public int CatalogItemId { get; set; }

    [Display(Name = "Purchase reception")]
    public PurchaseReception PurchaseReception { get; set; }

    [Display(Name = "Purchase reception ID")]
    public int PurchaseReceptionId { get; set; }

    [Display(Name = "Product name")]
    public string ProductName { get; set; }

    [Display(Name = "Product type")]
    public string ProductType { get; set; }

    [Display(Name = "Quantity")]
    public int Quantity { get; set; }

    [Display(Name = "Is satisfied")]
    public bool IsSatisfied { get; set; }
}
