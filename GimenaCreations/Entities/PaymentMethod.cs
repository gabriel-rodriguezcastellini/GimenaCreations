using System.ComponentModel.DataAnnotations;

namespace GimenaCreations.Entities;

public enum PaymentMethod
{
    [Display(Name = "Cash")]
    Cash = 1,

    [Display(Name = "Wire transfer")]
    WireTransfer = 2,

    [Display(Name = "Mercado Pago")]
    MercadoPago = 3
}
