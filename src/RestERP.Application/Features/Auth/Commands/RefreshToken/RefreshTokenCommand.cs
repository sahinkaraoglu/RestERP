using MediatR;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResponse?>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse?>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<TokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            => _authService.RefreshTokenAsync(request.RefreshToken);
    }
}
