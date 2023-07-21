using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public enum OrderStatus
{
    [Display(Name = "Submited")]
    Submited = 1,

    [Display(Name = "Paid")]
    Paid = 2,

    [Display(Name = "Cancelled")]
    Cancelled = 3,

    [Display(Name = "Shipped")]
    Shipped = 4
}
