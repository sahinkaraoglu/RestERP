using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(Order Order) : IRequest<Order>;

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            => _orderService.UpdateOrderAsync(request.Order);
    }
}
