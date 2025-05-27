using RestERP.Core.Doman.Entities.Base;

namespace RestERP.Core.Doman.Entities
{
    /// <summary>
    /// Rezervasyon entity sınıfı
    /// </summary>
    public class Reservation : BaseEntity
    {
        public string Name { get; set; } // Rezervasyon Yapan Kişinin Adı
        public string Phone { get; set; } // Telefon Numarası
        public DateTime Date { get; set; } // Tarih
        public string Time { get; set; } // Saat
        public int Guests { get; set; } // Misafir Sayısı
        public string? Notes { get; set; } // Notlar (nullable olarak işaretlendi)
    }
} 