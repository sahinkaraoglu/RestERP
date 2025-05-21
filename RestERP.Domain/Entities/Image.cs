using RestERP.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestERP.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string Path { get; set; }
        public int FoodId { get; set; }
        
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
} 