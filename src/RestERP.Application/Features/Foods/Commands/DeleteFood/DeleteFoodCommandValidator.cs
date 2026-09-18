using FluentValidation;

namespace RestERP.Application.Features.Foods.Commands.DeleteFood
{
    public class DeleteFoodCommandValidator : AbstractValidator<DeleteFoodCommand>
    {
        public DeleteFoodCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Geçerli bir yemek ID'si giriniz");
        }
    }
}
