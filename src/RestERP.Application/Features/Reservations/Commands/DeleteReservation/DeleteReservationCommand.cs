using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Reservations.Commands.DeleteReservation
{
    public record DeleteReservationCommand(int Id) : IRequest;

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand>
    {
        private readonly IReservationService _reservationService;

        public DeleteReservationCommandHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
            => _reservationService.DeleteReservationAsync(request.Id);
    }
}
