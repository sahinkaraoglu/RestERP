using FluentValidation;

namespace RestERP.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Giriş bilgileri zorunludur");

            When(x => x.Request != null, () =>
            {
                RuleFor(x => x.Request.Email)
                    .NotEmpty()
                    .WithMessage("Email zorunludur")
                    .EmailAddress()
                    .WithMessage("Geçerli bir email adresi giriniz");

                RuleFor(x => x.Request.Password)
                    .NotEmpty()
                    .WithMessage("Şifre zorunludur")
                    .MinimumLength(6)
                    .WithMessage("Şifre en az 6 karakter olmalıdır");
            });
        }
    }
}
