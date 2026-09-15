using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<IEnumerable<ApplicationUser>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<ApplicationUser>>
    {
        private readonly IUserService _userService;

        public GetUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<IEnumerable<ApplicationUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            => _userService.GetAllUsersAsync();
    }
}
