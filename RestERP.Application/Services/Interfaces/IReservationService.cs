using RestERP.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int id);
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<List<Reservation>> GetAllReservationsAsync();
    }
}
