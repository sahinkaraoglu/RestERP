using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Queries.GetUserByUsername
{
    public record GetUserByUsernameQuery(string Username) : IRequest<ApplicationUser?>;

    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, ApplicationUser?>
    {
        private readonly IUserService _userService;

        public GetUserByUsernameQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<ApplicationUser?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
            => _userService.GetUserByUsernameAsync(request.Username);
    }
}
