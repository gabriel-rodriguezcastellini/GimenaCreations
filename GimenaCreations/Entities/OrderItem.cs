using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }

    [Display(Name = "Catalog ")]
    public int CatalogItemId { get; set; }

    public CatalogItem CatalogItem { get; set; }

    [Display(Name = "Product name")]
    public string ProductName { get; set; }

    [Display(Name = "Picture")]
    public string PictureUrl { get; set; }

    [Display(Name = "Unit price")]
    public decimal UnitPrice { get; set; }

    public int Units { get; set; }

    [Display(Name = "Files")]
    public bool RequiresFile { get; set; }

    public ICollection<File> Files { get; set; }

    [NotMapped, Display(Name = "File")]
    public IFormFile FormFile { get; set; }

    [NotMapped]
    public decimal Total => UnitPrice * Units;
}
