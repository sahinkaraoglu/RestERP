using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<ApplicationUser?>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser?>
    {
        private readonly IUserService _userService;

        public GetUserByIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<ApplicationUser?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            => _userService.GetUserByIdAsync(request.Id);
    }
}
