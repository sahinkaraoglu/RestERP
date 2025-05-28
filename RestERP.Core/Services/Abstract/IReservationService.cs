using RestERP.Core.Doman.Entities;

namespace RestERP.Core.Services.Abstract
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        //Task<bool> ApproveReservationAsync(int id);
        //Task<bool> CancelReservationAsync(int id);
    }
} 