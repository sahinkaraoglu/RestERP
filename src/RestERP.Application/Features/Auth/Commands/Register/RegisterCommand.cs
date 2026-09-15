using MediatR;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(RegisterRequest Request) : IRequest<TokenResponse?>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenResponse?>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<TokenResponse?> Handle(RegisterCommand request, CancellationToken cancellationToken)
            => _authService.RegisterAsync(request.Request);
    }
}
