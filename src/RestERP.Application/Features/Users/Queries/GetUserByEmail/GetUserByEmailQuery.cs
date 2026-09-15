using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<ApplicationUser?>;

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApplicationUser?>
    {
        private readonly IUserService _userService;

        public GetUserByEmailQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<ApplicationUser?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            => _userService.GetUserByEmailAsync(request.Email);
    }
}
