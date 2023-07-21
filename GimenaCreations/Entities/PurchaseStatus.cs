using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public enum PurchaseStatus
{
    [Display(Name = "Submitted")]
    Submitted,

    [Display(Name = "Partially delivered")]
    PartiallyDelivered,

    [Display(Name = "Changing items")]
    ChangingItems,

    [Display(Name = "Delivered")]
    Delivered
}
