using Common.Contracts;
using MassTransit;

namespace General.Consumer.Consumers;

public class CreateCustomerBusQueryConsumer : IConsumer<CreateCustomerBusQuery>
{
    public async Task Consume(ConsumeContext<CreateCustomerBusQuery> context)
    {
        Console.WriteLine($"Получено сообщение: {context.Message.AccountId}");
        await Task.CompletedTask;
    }
}