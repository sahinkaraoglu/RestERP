using FluentValidation;

namespace RestERP.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.User)
                .NotNull()
                .WithMessage("Kullanıcı bilgisi zorunludur");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Şifre zorunludur")
                .MinimumLength(6)
                .WithMessage("Şifre en az 6 karakter olmalıdır");

            When(x => x.User != null, () =>
            {
                RuleFor(x => x.User.FirstName)
                    .NotEmpty()
                    .WithMessage("Ad zorunludur")
                    .MaximumLength(100);

                RuleFor(x => x.User.LastName)
                    .NotEmpty()
                    .WithMessage("Soyad zorunludur")
                    .MaximumLength(100);

                RuleFor(x => x.User.UserName)
                    .NotEmpty()
                    .WithMessage("Kullanıcı adı zorunludur")
                    .MaximumLength(50);

                RuleFor(x => x.User.Email)
                    .NotEmpty()
                    .WithMessage("Email zorunludur")
                    .EmailAddress()
                    .WithMessage("Geçerli bir email adresi giriniz")
                    .MaximumLength(100);

                RuleFor(x => x.User.PhoneNumber)
                    .MaximumLength(20)
                    .When(x => !string.IsNullOrWhiteSpace(x.User.PhoneNumber));

                RuleFor(x => x.User.Address)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.User.Address));
            });
        }
    }
}
