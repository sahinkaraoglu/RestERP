using Microsoft.EntityFrameworkCore;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Services.Abstract;
using RestERP.Domain.Interfaces;
using System.Linq.Expressions;

namespace RestERP.Core.Services.Concrete
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IRepository<Reservation> reservationRepository, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            var reservations = await _reservationRepository.GetAsync(
                predicate: r => true,
                orderBy: q => q.OrderByDescending(r => r.Date).ThenByDescending(r => r.Time)
            );
            return reservations.ToList();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        //public async Task<bool> ApproveReservationAsync(int id)
        //{
        //    var reservation = await _reservationRepository.GetByIdAsync(id);
        //    if (reservation == null)
        //        return false;

        //    reservation.Status = "Onaylandı";
        //    await _reservationRepository.UpdateAsync(reservation);
        //    await _unitOfWork.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> CancelReservationAsync(int id)
        //{
        //    var reservation = await _reservationRepository.GetByIdAsync(id);
        //    if (reservation == null)
        //        return false;

        //    reservation.Status = "İptal";
        //    await _reservationRepository.UpdateAsync(reservation);
        //    await _unitOfWork.SaveChangesAsync();
        //    return true;
        //}
    }
} 