using FluentValidation;

namespace RestERP.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kullanıcı ID'si giriniz");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Yeni şifre zorunludur")
                .MinimumLength(6)
                .WithMessage("Şifre en az 6 karakter olmalıdır");
        }
    }
}
