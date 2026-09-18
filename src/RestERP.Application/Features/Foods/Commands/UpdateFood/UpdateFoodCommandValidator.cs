using FluentValidation;
using RestERP.Application.Validators;

namespace RestERP.Application.Features.Foods.Commands.UpdateFood
{
    public class UpdateFoodCommandValidator : AbstractValidator<UpdateFoodCommand>
    {
        public UpdateFoodCommandValidator()
        {
            RuleFor(x => x.Food)
                .NotNull()
                .WithMessage("Yemek bilgisi zorunludur");

            When(x => x.Food != null, () =>
            {
                RuleFor(x => x.Food.Id)
                    .GreaterThan(0)
                    .WithMessage("Geçerli bir yemek ID'si giriniz");

                RuleFor(x => x.Food)
                    .SetValidator(new FoodValidator());
            });
        }
    }
}
