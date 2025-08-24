using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Yeni")]
        New = 0,

        [Display(Name = "Haz�rlan�yor")]
        InProgress = 1,

        [Display(Name = "Tamamland�")]
        Completed = 2,

        [Display(Name = "�ptal Edildi")]
        Cancelled = 3,

        [Display(Name = "Haz�r")]
        Ready = 4
    }
} 