using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(int Id) : IRequest<bool>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            => _userService.DeleteUserAsync(request.Id);
    }
}
