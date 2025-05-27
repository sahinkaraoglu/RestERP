using RestERP.Core.Doman.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestERP.Core.Doman.Entities
{
    public class Food : BaseEntity
    {
        public string Name { get; set; }
        public string TurkishName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
  
        [ForeignKey("CategoryId")]
        public FoodCategory Category { get; set; }
        
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
} 