using MediatR;
using RestERP.Application.DTOs;
using RestERP.Application.Logging;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(LoginRequest Request) : IRequest<TokenResponse?>, INoLogRequest;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse?>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<TokenResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
            => _authService.LoginAsync(request.Request);
    }
}
