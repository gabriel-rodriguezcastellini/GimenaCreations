using MassTransit;

namespace GimenaCreations.Consumers;

public class FileUploadedConsumerConfiguration : ConsumerDefinition<FileUploadedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileUploadedConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
    }
}
