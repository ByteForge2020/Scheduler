using Authentication.Application.Dto;
using Authentication.Application.Services.JwtService;
using Authentication.Infrastructure.Data;
using Authentication.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Commands.LogIn
{
    public class LogInHandler : IRequestHandler<LogInCommand, TokenPair>
    {
        private readonly UserManager<SchedulerUser> _userManager;
        private readonly SignInManager<SchedulerUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly AuthenticationDbContext _authenticationDbContext;
        private readonly IMediator _mediator;

        public LogInHandler(
            UserManager<SchedulerUser> userManager,
            SignInManager<SchedulerUser> signInManager,
            IJwtService jwtService,
            AuthenticationDbContext authenticationDbContext,
            IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _authenticationDbContext = authenticationDbContext;
            _mediator = mediator;
        }

        public async Task<TokenPair> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _jwtService.GenerateTokenPair(request, cancellationToken);
            
            return tokens;
        }
    }
}