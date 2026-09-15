using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery : IRequest<ApplicationUser?>;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, ApplicationUser?>
    {
        private readonly IUserService _userService;

        public GetCurrentUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<ApplicationUser?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
            => _userService.GetCurrentUserAsync();
    }
}
