using Customer.Application.Dto;
using Customer.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customer.Application.Queries.GetCustomers
{
    public record GetCustomersQuery(Guid AccountId, int Page, int PageSize)
        : IRequest<GetCustomersQuery.Result>
    {
        public record Result(List<CustomerDto> Customers, int TotalCount);

        public class Handler : IRequestHandler<GetCustomersQuery, Result>
        {
            private readonly ILogger<Handler> _logger;
            private readonly CustomerDbContext _dbContext;

            public Handler(ILogger<Handler> logger, CustomerDbContext dbContext)
            {
                _logger = logger;
                _dbContext = dbContext;
            }

            public async Task<Result> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                var query = _dbContext.Customers
                    .Where(c => c.AccountId == request.AccountId);
                
                var totalCount = await query.CountAsync(cancellationToken);
                
                var customers = await query
                    .Skip(request.Page * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Surname = c.Surname,
                        Phone = c.Phone
                    })
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Retrieved {Count} customers for AccountId {AccountId} (Page {Page}, Size {PageSize})", customers.Count, request.AccountId, request.Page, request.PageSize);

                return new Result(Customers: customers, TotalCount: totalCount);
            }
        }
    }
}
