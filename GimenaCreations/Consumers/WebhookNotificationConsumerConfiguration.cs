using MassTransit;

namespace GimenaCreations.Consumers;

public class WebhookNotificationConsumerConfiguration : ConsumerDefinition<WebhookNotificationConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<WebhookNotificationConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
    }
}
