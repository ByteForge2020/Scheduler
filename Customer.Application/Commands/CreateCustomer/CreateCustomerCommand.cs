using Common.Contracts;
using Customer.Application.Dto;
using Customer.Infrastructure;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customer.Application.Commands.CreateCustomer
{
    public record CreateCustomerCommand(Guid AccountId, string Name, string Phone, string Surname)
        : IRequest<CreateCustomerCommand.Result>
    {
        public record Result(CustomerDto CustomerDto);

        public class Handler : IRequestHandler<CreateCustomerCommand, Result>
        {
            private readonly ILogger<Handler> _logger;
            private readonly CustomerDbContext _dbContext;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ILogger<Handler> logger, CustomerDbContext dbContext, IPublishEndpoint publishEndpoint)
            {
                _logger = logger;
                _dbContext = dbContext;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Core.Entities.Customer
                {
                    AccountId = request.AccountId,
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Phone = request.Phone,
                    Surname = request.Surname
                };

                await _dbContext.Customers.AddAsync(customer, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _publishEndpoint.Publish(new CreateCustomerBusQuery
                    {
                        Surname = customer.Surname,
                        Name = customer.Surname,
                        Phone = customer.Phone,
                        Id = customer.Id,
                        AccountId = customer.AccountId
                    },
                    cancellationToken);
                return new Result(new CustomerDto
                {
                    Surname = customer.Surname,
                    Phone = customer.Phone,
                    Id = customer.Id,
                    Name = customer.Name
                });
            }
        }
    }
}