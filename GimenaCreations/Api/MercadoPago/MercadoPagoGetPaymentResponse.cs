using System.Text.Json.Serialization;

namespace GimenaCreations.Api.MercadoPago;

public class MercadoPagoGetPaymentResponse
{
    public string Status { get; set; }

    [JsonPropertyName("external_reference")]
    public string ExternalReference { get; set; }
}