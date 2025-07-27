using RestERP.Core.Domain.Entities.Base;

namespace RestERP.Core.Domain.Entities
{
    /// <summary>
    /// Restoran masası entity sınıfı
    /// </summary>
    public class Table : BaseEntity
    {
        public bool IsOccupied { get; set; } = false;
    }
} 