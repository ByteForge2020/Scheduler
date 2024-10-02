using MediatR;
using Microsoft.Extensions.Logging;

namespace General.Application.Queries.GetCustomers
{
    public record TestQuery : IRequest<TestQuery.Result>
    {
        public record Result(bool Res);
    
        public class Handler : IRequestHandler<TestQuery, Result>
        {
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger;
            }

            public async Task<Result> Handle(TestQuery request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handling TestQuery request.");
                return new Result(Res: true);
            }
        }
    }
}