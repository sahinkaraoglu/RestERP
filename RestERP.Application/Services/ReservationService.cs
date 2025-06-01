using RestERP.Application.Services.Interfaces;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            ValidateReservation(reservation);

            // Aynı tarih ve saatte başka rezervasyon var mı kontrolü
            var existingReservations = await _unitOfWork.Repository<Reservation>().GetAllAsync();
            if (existingReservations.Any(r => 
                r.Date.Date == reservation.Date.Date && 
                r.Time == reservation.Time))
            {
                throw new InvalidOperationException("Bu tarih ve saatte başka bir rezervasyon bulunmaktadır.");
            }

            await _unitOfWork.Repository<Reservation>().AddAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
            return reservation;
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            ValidateReservation(reservation);

            var existingReservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(reservation.Id);
            if (existingReservation == null)
                throw new KeyNotFoundException($"ID: {reservation.Id} olan rezervasyon bulunamadı.");

            await _unitOfWork.Repository<Reservation>().UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
            if (reservation == null)
                throw new KeyNotFoundException($"ID: {id} olan rezervasyon bulunamadı.");

            await _unitOfWork.Repository<Reservation>().DeleteAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
            if (reservation == null)
                throw new KeyNotFoundException($"ID: {id} olan rezervasyon bulunamadı.");

            return reservation;
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var result = await _unitOfWork.Repository<Reservation>().GetAllAsync();
            return new List<Reservation>(result);
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
