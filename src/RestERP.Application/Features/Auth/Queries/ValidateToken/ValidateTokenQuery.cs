using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Auth.Queries.ValidateToken
{
    public record ValidateTokenQuery(string Token) : IRequest<bool>;

    public class ValidateTokenQueryHandler : IRequestHandler<ValidateTokenQuery, bool>
    {
        private readonly IAuthService _authService;

        public ValidateTokenQueryHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<bool> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
            => _authService.ValidateTokenAsync(request.Token);
    }
}
