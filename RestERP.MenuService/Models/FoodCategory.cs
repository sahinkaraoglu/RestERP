using System.Collections.Generic;

namespace RestERP.MenuService.Models
{
    public class FoodCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        
        // Navigation property
        public ICollection<Food>? Foods { get; set; }
    }
} 