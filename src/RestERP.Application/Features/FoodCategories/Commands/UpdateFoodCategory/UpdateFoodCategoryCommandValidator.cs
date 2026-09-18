using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.FoodCategories.Commands.UpdateFoodCategory
{
    public class UpdateFoodCategoryCommandValidator : AbstractValidator<UpdateFoodCategoryCommand>
    {
        public UpdateFoodCategoryCommandValidator()
        {
            RuleFor(x => x.Category)
                .NotNull()
                .WithMessage("Kategori bilgisi zorunludur");

            When(x => x.Category != null, () =>
            {
                RuleFor(x => x.Category.Id)
                    .GreaterThan(0)
                    .WithMessage("Geçerli bir kategori ID'si giriniz");

                RuleFor(x => x.Category)
                    .SetValidator(new FoodCategoryValidator());
            });
        }
    }
}
