using Common.Authorization.AuthorizationAttributes;
using Customer.Application.Commands.CreateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AccountIdAuthorization]
        public async Task<ActionResult<CreateCustomerCommand.Result>> CreateCustomer([FromBody] CreateCustomerCommand command, CancellationToken ct)
        {
            if (HttpContext.Items["AccountId"] is not Guid accountId)
            {
                return Unauthorized();
            }
            var result = await _mediator.Send(new CreateCustomerCommand(accountId, command.Name, command.Phone, command.Surname), ct);
            return result;
        }
    }
}