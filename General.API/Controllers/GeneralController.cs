using Common.Authorization;
using Common.Authorization.AuthorizationAttributes;
using General.Application.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace General.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorization _authorizationContext;

        public GeneralController(IMediator mediator, IAuthorization authorizationContext)
        {
            _mediator = mediator;
            _authorizationContext = authorizationContext;
        }

        [HttpGet]
        [AccountIdAuthorization]
        public async Task<ActionResult<TestQuery.Result>> Test(CancellationToken ct)
        {
            var command = new TestQuery(_authorizationContext.ThrowOrGetAccountId());
            return await _mediator.Send(command, ct);
        }
    }
}