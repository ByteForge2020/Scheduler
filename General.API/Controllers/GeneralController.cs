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

        public GeneralController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<TestResponse>> Test([FromQuery] TestQuery command, CancellationToken ct)
        {
            return await _mediator.Send(command, ct);
        }
        

    }
}