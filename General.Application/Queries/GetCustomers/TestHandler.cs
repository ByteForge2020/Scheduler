using MediatR;
using Microsoft.Extensions.Logging;

namespace General.Application.Queries.GetCustomers
{
    public record TestQuery : IRequest<TestResponse>;
    public record TestResponse(bool Result);

    // Обработчик
    public class TestHandler : IRequestHandler<TestQuery, TestResponse>
    {
        private readonly ILogger<TestHandler> _logger;

        public TestHandler(ILogger<TestHandler> logger)
        {
            _logger = logger;
        }

        public async Task<TestResponse> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("This is a test exception");
            _logger.LogInformation("Handling TestQuery request.");
            // throw new InvalidOperationException("This is a test exception");
            return new TestResponse(true);
        }
    }
}