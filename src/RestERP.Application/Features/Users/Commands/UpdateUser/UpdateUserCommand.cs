using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(ApplicationUser User) : IRequest<bool>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            => _userService.UpdateUserAsync(request.User);
    }
}
