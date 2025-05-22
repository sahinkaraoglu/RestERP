using System.ComponentModel.DataAnnotations;

namespace RestERP.Domain.Enums
{
       public enum Role
    {
        [Display(Name = "M��teri")]
        Customer = 1,

        [Display(Name = "Personel")]
        Employee = 2,
    }
}
