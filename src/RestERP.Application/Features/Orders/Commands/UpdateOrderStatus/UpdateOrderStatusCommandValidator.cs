using FluentValidation;

namespace RestERP.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir sipariş ID'si giriniz");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Geçerli bir sipariş durumu seçiniz");
        }
    }
}
