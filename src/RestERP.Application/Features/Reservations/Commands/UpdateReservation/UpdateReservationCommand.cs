using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Reservations.Commands.UpdateReservation
{
    public record UpdateReservationCommand(Reservation Reservation) : IRequest;

    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand>
    {
        private readonly IReservationService _reservationService;

        public UpdateReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Task Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
            => _reservationService.UpdateReservationAsync(request.Reservation);
    }
}
