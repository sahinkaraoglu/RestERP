using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.Foods.Commands.CreateFood
{
    public class CreateFoodCommandValidator : AbstractValidator<CreateFoodCommand>
    {
        public CreateFoodCommandValidator()
        {
            RuleFor(x => x.Food)
                .NotNull()
                .WithMessage("Yemek bilgisi zorunludur")
                .SetValidator(new FoodValidator());
        }
    }
}
