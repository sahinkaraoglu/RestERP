using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Reservations.Queries.GetReservations
{
    public record GetReservationsQuery : IRequest<List<Reservation>>;

    public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, List<Reservation>>
    {
        private readonly IReservationService _reservationService;

        public GetReservationsQueryHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Task<List<Reservation>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
            => _reservationService.GetAllReservationsAsync();
    }
}
