using MassTransit;

namespace GimenaCreations.Consumers;

public class OrderStatusChangedToSubmittedConsumerConfiguration : ConsumerDefinition<OrderStatusChangedToSubmittedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderStatusChangedToSubmittedConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
    }
}
