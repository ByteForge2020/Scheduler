using Customer.Contracts;
using General.Application.Queries.GetCustomers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace General.Application.Queries.GetCustomersByGrpc
{
    public record GetCustomersByGrpc(Guid AccountId) : IRequest<GetCustomersByGrpc.Result>
    {
        public record Result(GetCustomersResponse Res);
    
        public class Handler : IRequestHandler<GetCustomersByGrpc, Result>
        {
            private readonly ILogger<Handler> _logger;
            private readonly CustomerService.CustomerServiceClient _client;
            public Handler(ILogger<Handler> logger, CustomerService.CustomerServiceClient client)
            {
                _logger = logger;
                _client = client;
            }

            public async Task<Result> Handle(GetCustomersByGrpc request, CancellationToken cancellationToken)
            {
                var customersRequest = new GetCustomersRequest { AccountId = request.AccountId.ToString() };
                var response = await _client.GetCustomersAsync(customersRequest);
                return new Result(Res: response);
            }
        }
    }
}