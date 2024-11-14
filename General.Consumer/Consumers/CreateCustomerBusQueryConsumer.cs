using Common.Contracts;
using General.Application.Commands.CreateCustomer;
using MassTransit;
using MassTransit;
using MediatR;


namespace General.Consumer.Consumers;
public class CreateCustomerBusQueryConsumer : IConsumer<CreateCustomerBusQuery>
{
    private readonly IMediator _mediator;

    public CreateCustomerBusQueryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CreateCustomerBusQuery> context)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(
            context.Message.AccountId,
            context.Message.Id,
            context.Message.Name), context.CancellationToken);
        await context.RespondAsync(new CreateCustomerCommand.Result(result.IsSuccess));
    }
}