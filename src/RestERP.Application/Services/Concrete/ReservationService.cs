using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
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
            _unitOfWork = unitOfWork;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await _unitOfWork.Repository<Reservation>().AddAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
            return reservation;
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            await _unitOfWork.Repository<Reservation>().UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(int id)
        {
            var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
            if (reservation != null)
            {
                await _unitOfWork.Repository<Reservation>().DeleteAsync(reservation);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            var result = await _unitOfWork.Repository<Reservation>().GetAllAsync();
            return result.ToList();
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            var result = await _unitOfWork.Repository<Reservation>().GetAllAsync();
            return result.ToList();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
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
