using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;

namespace RestERP.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int OrderId, OrderStatus Status) : IRequest<bool>;

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderStatusCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            => _orderService.UpdateOrderStatusAsync(request.OrderId, request.Status);
    }
}
