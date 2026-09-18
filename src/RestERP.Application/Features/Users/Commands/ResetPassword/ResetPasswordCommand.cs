using MediatR;
using RestERP.Application.Logging;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(int UserId, string NewPassword) : IRequest<UserCommandResult>, INoLogRequest;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, UserCommandResult>
    {
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<UserCommandResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            => _userService.ResetPasswordAsync(request.UserId, request.NewPassword);
    }
}
