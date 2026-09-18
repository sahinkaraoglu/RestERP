using FluentValidation;

namespace RestERP.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Kayıt bilgileri zorunludur");

            When(x => x.Request != null, () =>
            {
                RuleFor(x => x.Request.FirstName)
                    .NotEmpty()
                    .WithMessage("Ad zorunludur")
                    .MaximumLength(100);

                RuleFor(x => x.Request.LastName)
                    .NotEmpty()
                    .WithMessage("Soyad zorunludur")
                    .MaximumLength(100);

                RuleFor(x => x.Request.UserName)
                    .NotEmpty()
                    .WithMessage("Kullanıcı adı zorunludur")
                    .MaximumLength(50);

                RuleFor(x => x.Request.Email)
                    .NotEmpty()
                    .WithMessage("Email zorunludur")
                    .EmailAddress()
                    .WithMessage("Geçerli bir email adresi giriniz")
                    .MaximumLength(100);

                RuleFor(x => x.Request.PhoneNumber)
                    .MaximumLength(20)
                    .When(x => !string.IsNullOrWhiteSpace(x.Request.PhoneNumber));

                RuleFor(x => x.Request.Address)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Request.Address));

                RuleFor(x => x.Request.Password)
                    .NotEmpty()
                    .WithMessage("Şifre zorunludur")
                    .MinimumLength(6)
                    .WithMessage("Şifre en az 6 karakter olmalıdır");

                RuleFor(x => x.Request.ConfirmPassword)
                    .Equal(x => x.Request.Password)
                    .WithMessage("Şifreler eşleşmiyor");
            });
        }
    }
}
