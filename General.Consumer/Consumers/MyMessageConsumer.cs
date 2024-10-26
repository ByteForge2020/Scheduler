using MassTransit;

namespace General.Consumer.Consumers;
public class MyMessage
{
    public string Text { get; set; }
}

public class MyMessageConsumer : IConsumer<MyMessage>
{
    public async Task Consume(ConsumeContext<MyMessage> context)
    {
        Console.WriteLine($"Получено сообщение: {context.Message.Text}");
        await Task.CompletedTask;
    }
}