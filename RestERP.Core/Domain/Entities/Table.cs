using RestERP.Core.Doman.Entities.Base;

namespace RestERP.Core.Doman.Entities
{
    /// <summary>
    /// Restoran masası entity sınıfı
    /// </summary>
    public class Table : BaseEntity
    {
        public bool IsOccupied { get; set; } = false;
    }
} 