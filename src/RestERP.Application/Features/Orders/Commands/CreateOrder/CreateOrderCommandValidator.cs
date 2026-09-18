using FluentValidation;

namespace RestERP.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order)
                .NotNull()
                .WithMessage("Sipariş bilgisi zorunludur");

            When(x => x.Order != null, () =>
            {
                RuleFor(x => x.Order.OrderItems)
                    .NotEmpty()
                    .WithMessage("Sipariş en az bir ürün içermelidir");

                RuleForEach(x => x.Order.OrderItems).ChildRules(item =>
                {
                    item.RuleFor(i => i.FoodId)
                        .GreaterThan(0)
                        .WithMessage("Geçerli bir ürün seçiniz");

                    item.RuleFor(i => i.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Ürün miktarı en az 1 olmalıdır");
                });
            });
        }
    }
}
