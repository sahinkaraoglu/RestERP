using FluentValidation;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Validators
{
    public class FoodValidator : AbstractValidator<Food>
    {
        public FoodValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Yemek adı zorunludur")
                .MaximumLength(200);

            RuleFor(x => x.TurkishName)
                .NotEmpty()
                .WithMessage("Türkçe yemek adı zorunludur")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kategori seçiniz");
        }
    }
}
