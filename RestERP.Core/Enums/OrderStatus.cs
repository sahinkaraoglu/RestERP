using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Yeni")]
        New = 0,

        [Display(Name = "Hazýrlanýyor")]
        InProgress = 1,

        [Display(Name = "Tamamlandý")]
        Completed = 2,

        [Display(Name = "Ýptal Edildi")]
        Cancelled = 3,

        [Display(Name = "Hazýr")]
        Ready = 4
    }
} 