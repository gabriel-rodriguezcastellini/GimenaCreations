using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimenaCreations.Entities;

public class Supplier
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Supplier ID")]
    public string SupplierId { get; set; }

    public bool Agreement { get; set; }

    [Required]
    [Display(Name = "Supplier type")]
    public string SupplierType { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone 1")]
    public string Phone1 { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone 2")]
    public string Phone2 { get; set; }

    [Required]
    [Display(Name = "Contact name")]
    public string ContactName { get; set; }

    [Phone]
    [Display(Name = "Contact phone")]
    public string ContactPhone { get; set; }

    [Required]
    [Display(Name = "Business name")]
    public string BusinessName { get; set; }

    [Required]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [Required]
    [Display(Name = "City")]
    public string City { get; set; }

    [Required]
    [Display(Name = "State")]
    public string State { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Date added")]
    public DateTime AddDate { get; set; }

    public byte[] Image { get; set; }

    [NotMapped]
    [Display(Name = "Image")]
    public IFormFile FormFile { get; set; }    
}
