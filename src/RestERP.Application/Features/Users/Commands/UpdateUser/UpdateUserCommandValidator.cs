using FluentValidation;

namespace RestERP.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.User)
                .NotNull()
                .WithMessage("Kullanıcı bilgisi zorunludur");

            When(x => x.User != null, () =>
            {
                RuleFor(x => x.User.Id)
                    .GreaterThan(0)
                    .WithMessage("Geçerli bir kullanıcı ID'si giriniz");

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
