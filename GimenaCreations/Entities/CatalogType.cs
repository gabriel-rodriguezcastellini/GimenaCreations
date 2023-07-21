using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities
{
    [Display(Name = "Catalog type")]
    public class CatalogType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
