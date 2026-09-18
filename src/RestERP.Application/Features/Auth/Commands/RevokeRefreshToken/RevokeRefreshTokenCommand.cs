using MediatR;
using RestERP.Application.Logging;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Auth.Commands.RevokeRefreshToken
{
    public record RevokeRefreshTokenCommand(string RefreshToken) : IRequest<bool>, INoLogRequest;

    public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
    {
        private readonly IAuthService _authService;

        public RevokeRefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<bool> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
            => _authService.RevokeRefreshTokenAsync(request.RefreshToken);
    }
}
