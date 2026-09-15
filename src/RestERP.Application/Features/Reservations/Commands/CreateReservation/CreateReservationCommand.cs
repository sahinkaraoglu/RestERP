using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Reservations.Commands.CreateReservation
{
    public record CreateReservationCommand(Reservation Reservation) : IRequest<Reservation>;

    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Reservation>
    {
        private readonly IReservationService _reservationService;

        public CreateReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Task<Reservation> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
            => _reservationService.CreateReservationAsync(request.Reservation);
    }
}
