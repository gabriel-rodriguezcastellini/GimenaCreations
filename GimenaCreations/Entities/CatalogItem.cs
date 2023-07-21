using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities
{
    public class CatalogItem
    {
        public int Id { get; set; }

        [Display(Name = "Product name")]
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        [Display(Name = "Picture")]
        public string PictureFileName { get; set; }

        [Display(Name = "Catalog type")]
        public int CatalogTypeId { get; set; }

        [Display(Name = "Catalog type")]
        public CatalogType CatalogType { get; set; }

        [Display(Name = "Available stock")]
        public int AvailableStock { get; set; }

        [Display(Name = "Restock threshold")]
        public int RestockThreshold { get; set; }

        [Display(Name = "Max stock threshold")]
        public int MaxStockThreshold { get; set; }

        [Display(Name = "On reorder")]
        public bool OnReorder { get; set; }

        [Display(Name = "Requires files")]
        public bool RequiresFile { get; set; }

        [Display(Name = "Visible")]
        public bool IsVisible { get; set; }

        public ICollection<OrderItem> Items { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        [NotMapped, Display(Name = "Upload new picture")]
        public IFormFile FormFile { get; set; }
    }
}
