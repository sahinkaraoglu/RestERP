using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.Reservation)
                .NotNull()
                .WithMessage("Rezervasyon bilgisi zorunludur")
                .SetValidator(new ReservationValidator());
        }
    }
}
