using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;

namespace RestERP.Application.Services.Concrete
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
            return reservation;
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            _reservationRepository.Update(reservation);
            await _reservationRepository.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation != null)
            {
                _reservationRepository.Delete(reservation);
                await _reservationRepository.SaveChangesAsync();
            }
        }

        public Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return _reservationRepository.GetByIdAsync(id);
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var result = await _reservationRepository.GetAllAsync();
            return result.ToList();
        }
    }
}
