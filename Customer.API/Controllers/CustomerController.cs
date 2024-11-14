using Common.Authorization;
using Common.Authorization.AuthorizationAttributes;
using Customer.Application.Commands.CreateCustomer;
using Customer.Application.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorization _authorizationContext;

        public CustomerController(IMediator mediator, IAuthorization authorizationContext)
        {
            _mediator = mediator;
            _authorizationContext = authorizationContext;
        }

        [HttpPost]
        [AccountIdAuthorization]
        public async Task<ActionResult<CreateCustomerCommand.Result>> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken ct)
        {
            var accountId = _authorizationContext.ThrowOrGetAccountId();
            var result = await _mediator.Send(new CreateCustomerCommand(accountId, command.Name, command.Phone, command.Surname), ct);
            return result;
        }
        
        [HttpGet]
        [AccountIdAuthorization]
        public async Task<ActionResult<GetCustomersQuery.Result>> GetCustomers([FromQuery] GetCustomersQuery command, CancellationToken ct)
        {
            var accountId = _authorizationContext.ThrowOrGetAccountId();
            var result = await _mediator.Send(new GetCustomersQuery(accountId, command.Page, command.PageSize), ct);
            return result;
        }
    }
}