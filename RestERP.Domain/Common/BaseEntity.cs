namespace RestERP.Domain.Common
{
    /// <summary>
    /// Tüm entity'lerin türetileceği temel sınıf
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
} 