namespace RestERP.MenuService.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? ImageUrl { get; set; }
        
        // Navigation property
        public FoodCategory? Category { get; set; }
    }
} 