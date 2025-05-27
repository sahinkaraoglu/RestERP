using RestERP.Core.Doman.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestERP.Core.Doman.Entities
{
    public class Image : BaseEntity
    {
        public string Path { get; set; }
        public int FoodId { get; set; }
        
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
} 