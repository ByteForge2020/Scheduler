using General.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace General.Application.Commands.CreateCustomer
{
    public record CreateCustomerCommand(Guid AccountId, Guid CustomerId, string Name)
        : IRequest<CreateCustomerCommand.Result>
    {
        public record Result(bool IsSuccess);

        public class Handler : IRequestHandler<CreateCustomerCommand, Result>
        {
            private readonly ILogger<Handler> _logger;
            private readonly GeneralDbContext _dbContext;

            public Handler(ILogger<Handler> logger, GeneralDbContext dbContext)
            {
                _logger = logger;
                _dbContext = dbContext;
            }

            public async Task<Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Core.Entities.Customer
                {
                    AccountId = request.AccountId,
                    Id = request.CustomerId,
                    Name = request.Name,
                };

                await _dbContext.Customers.AddAsync(customer, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Result(IsSuccess: true);
            }
        }
    }
}