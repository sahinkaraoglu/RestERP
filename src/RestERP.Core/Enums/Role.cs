using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Enums
{
       public enum Role
    {
        [Display(Name = "Müþteri")]
        Customer = 1,

        [Display(Name = "Personel")]
        Employee = 2,
    }
}
