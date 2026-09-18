using FluentValidation;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Validators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Rezervasyon adı zorunludur")
                .MaximumLength(200);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Telefon numarası zorunludur")
                .MaximumLength(20);

            RuleFor(x => x.Date)
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Rezervasyon tarihi geçmiş olamaz");

            RuleFor(x => x.Time)
                .NotEmpty()
                .WithMessage("Rezervasyon saati zorunludur")
                .MaximumLength(10);

            RuleFor(x => x.Guests)
                .GreaterThan(0)
                .WithMessage("Misafir sayısı en az 1 olmalıdır")
                .LessThanOrEqualTo(100)
                .WithMessage("Misafir sayısı en fazla 100 olabilir");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }
}
