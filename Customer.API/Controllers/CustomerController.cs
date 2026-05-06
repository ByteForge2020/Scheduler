using Common.Authorization;
using Common.Authorization.AuthorizationAttributes;
using Customer.Application.Commands.CreateCustomer;
using Customer.Application.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<ActionResult<GetCustomersQuery.Result>> GetCustomers([FromQuery] GetCustomersQuery command, CancellationToken ct)
        {
            var accountId = Guid.Parse("5444BD37-707C-496D-8084-6FDE3DD9A176");
            var result = await _mediator.Send(new GetCustomersQuery(accountId, command.Page, command.PageSize), ct);
            return result;
        }
    }
}