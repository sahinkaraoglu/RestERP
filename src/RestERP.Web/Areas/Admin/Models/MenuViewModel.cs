using System;
using System.ComponentModel.DataAnnotations;

namespace RestERP.Web.Areas.Admin.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Türkçe ürün adı zorunludur")]
        public string TurkishName { get; set; }
        
        [Required(ErrorMessage = "İngilizce ürün adı zorunludur")]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Fiyat bilgisi zorunludur")]
        public decimal Price { get; set; }
        
        public int? PrepTime { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsVegan { get; set; }
        
        public bool IsVegetarian { get; set; }
        
        public bool IsGlutenFree { get; set; }
        
        public bool IsSpicy { get; set; }
    }
} 