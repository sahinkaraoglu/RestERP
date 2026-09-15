using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Reservations.Queries.GetReservationById
{
    public record GetReservationByIdQuery(int Id) : IRequest<Reservation?>;

    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, Reservation?>
    {
        private readonly IReservationService _reservationService;

        public GetReservationByIdQueryHandler(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public Task<Reservation?> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
            => _reservationService.GetReservationByIdAsync(request.Id);
    }
}
