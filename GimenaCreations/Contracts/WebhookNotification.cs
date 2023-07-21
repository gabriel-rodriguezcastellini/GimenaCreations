namespace GimenaCreations.Contracts;

public record WebhookNotification
{
    public string PaymentId { get; }

    public WebhookNotification(string paymentId) => PaymentId = paymentId;
}
