using GimenaCreations.MarkerInterfaces;
using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public class PurchaseReception : IAuditable
{
    [Display(Name = "ID")]
    public int Id { get; set; }    

    [Display(Name = "Purchase")]
    public Purchase Purchase { get; set; }

    [Display(Name = "Purchase ID")]
    public int PurchaseId { get; set; }

    [Display(Name = "Input number")]
    public string InputNumber { get; set; }

    [Display(Name = "Input date")]
    public DateTime InputDate { get; set; }

    [Display(Name = "Invoice number")]
    public string InvoiceNumber { get; set; }

    [Display(Name = "Invoice date")]
    public DateTime InvoiceDate { get; set; }

    [Display(Name = "Invoice reception date")]
    public DateTime InvoiceReceptionDate { get; set; }
    
    public List<PurchaseReceptionItem> PurchaseReceptionItems { get; set; }
}
