using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.Reservations.Commands.UpdateReservation
{
    public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
    {
        public UpdateReservationCommandValidator()
        {
            RuleFor(x => x.Reservation)
                .NotNull()
                .WithMessage("Rezervasyon bilgisi zorunludur");

            When(x => x.Reservation != null, () =>
            {
                RuleFor(x => x.Reservation.Id)
                    .GreaterThan(0)
                    .WithMessage("Geçerli bir rezervasyon ID'si giriniz");

                RuleFor(x => x.Reservation)
                    .SetValidator(new ReservationValidator());
            });
        }
    }
}
