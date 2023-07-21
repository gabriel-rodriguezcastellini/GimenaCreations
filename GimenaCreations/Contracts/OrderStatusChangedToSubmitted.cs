using GimenaCreations.Entities;

namespace GimenaCreations.Contracts;

public record OrderStatusChangedToSubmitted
{    
    public int OrderId { get; }
    public OrderStatus OrderStatus { get; }
    public string UserId { get; }

    public OrderStatusChangedToSubmitted(int orderId, OrderStatus orderStatus, string userId)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        UserId = userId;        
    }
}
