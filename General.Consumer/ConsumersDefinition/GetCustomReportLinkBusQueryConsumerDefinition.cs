
using General.Consumer.Consumers;
using MassTransit;

namespace General.Consumer.ConsumersDefinition;

public class GetCustomReportLinkBusQueryConsumerDefinition : ConsumerDefinition<MyMessageConsumer>
{
    public GetCustomReportLinkBusQueryConsumerDefinition()
    {
    }
}
