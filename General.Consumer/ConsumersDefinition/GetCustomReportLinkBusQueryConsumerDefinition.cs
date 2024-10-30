
using General.Consumer.Consumers;
using MassTransit;

namespace General.Consumer.ConsumersDefinition;

public class GetCustomReportLinkBusQueryConsumerDefinition : ConsumerDefinition<CreateCustomerBusQueryConsumer>
{
    public GetCustomReportLinkBusQueryConsumerDefinition()
    {
    }
}
