using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
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

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var result = await _reservationRepository.GetAllAsync();
            return result.ToList();
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            var result = await _reservationRepository.GetAllAsync();
            return result.ToList();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        private void ValidateReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            if (string.IsNullOrWhiteSpace(reservation.Name))
                throw new ArgumentException("Rezervasyon sahibinin adı boş olamaz.");

            if (string.IsNullOrWhiteSpace(reservation.Phone))
                throw new ArgumentException("Telefon numarası boş olamaz.");

            if (string.IsNullOrWhiteSpace(reservation.Time))
                throw new ArgumentException("Rezervasyon saati boş olamaz.");

            if (reservation.Date < DateTime.Today)
                throw new ArgumentException("Geçmiş bir tarihe rezervasyon yapılamaz.");

            if (reservation.Guests <= 0)
                throw new ArgumentException("Misafir sayısı 0'dan büyük olmalıdır.");
        }
    }
}
