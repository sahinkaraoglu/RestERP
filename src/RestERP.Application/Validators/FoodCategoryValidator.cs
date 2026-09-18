using FluentValidation;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Validators
{
    public class FoodCategoryValidator : AbstractValidator<FoodCategory>
    {
        public FoodCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Kategori adı zorunludur")
                .MaximumLength(200);

            RuleFor(x => x.TurkishName)
                .NotEmpty()
                .WithMessage("Türkçe kategori adı zorunludur")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
