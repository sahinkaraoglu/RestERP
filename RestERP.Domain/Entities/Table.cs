using RestERP.Domain.Entities.Base;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Restoran masası entity sınıfı
    /// </summary>
    public class Table : BaseEntity
    {
        public bool IsOccupied { get; set; } = false;
    }
} 