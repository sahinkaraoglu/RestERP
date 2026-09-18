using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.FoodCategories.Commands.CreateFoodCategory
{
    public class CreateFoodCategoryCommandValidator : AbstractValidator<CreateFoodCategoryCommand>
    {
        public CreateFoodCategoryCommandValidator()
        {
            RuleFor(x => x.Category)
                .NotNull()
                .WithMessage("Kategori bilgisi zorunludur")
                .SetValidator(new FoodCategoryValidator());
        }
    }
}
