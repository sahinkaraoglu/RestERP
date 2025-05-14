using RestERP.Domain.Common;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Restoran masası entity sınıfı
    /// </summary>
    public class Table : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 