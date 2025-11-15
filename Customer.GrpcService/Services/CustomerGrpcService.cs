using Customer.Contracts;
using Customer.Infrastructure;
using Grpc.Core;

namespace Customer.GrpcService.Services;

public class CustomerGrpcService : CustomerService.CustomerServiceBase
{
    private readonly CustomerDbContext _customerDbContext;
    private readonly ILogger<CustomerGrpcService> _logger;

    public CustomerGrpcService(CustomerDbContext customerDbContext, ILogger<CustomerGrpcService> logger)
    {
        _customerDbContext = customerDbContext;
        _logger = logger;
    }

    public override async Task<GetCustomersResponse> GetCustomers(GetCustomersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Getting customers for account {AccountId}", request.AccountId);
        
        try
        {
            var customers = _customerDbContext.Customers
                .Where(c => c.AccountId == Guid.Parse(request.AccountId));

            var response = new GetCustomersResponse();
            response.Customers.AddRange(customers.Select(customer => new CustomerItem
            {
                Id = customer.Id.ToString(),
                Name = customer.Name
            }));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customers for account {AccountId}", request.AccountId);
            throw new RpcException(new Status(StatusCode.Internal, "Failed to get customers"));
        }
    }
}