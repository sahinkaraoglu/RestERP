using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Abstract
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int id);
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task<List<Reservation>> GetAllReservationsAsync();
    }
}
