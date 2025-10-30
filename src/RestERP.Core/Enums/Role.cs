using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Enums
{
       public enum Role
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Personel")]
        Employee = 2,

        [Display(Name = "Müşteri")]
        Customer = 3,


    }
}
