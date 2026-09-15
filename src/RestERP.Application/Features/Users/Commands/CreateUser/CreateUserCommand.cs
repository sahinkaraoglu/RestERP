using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(ApplicationUser User, string Password) : IRequest<UserCommandResult>;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserCommandResult>
    {
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<UserCommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            => _userService.CreateUserAsync(request.User, request.Password);
    }
}
