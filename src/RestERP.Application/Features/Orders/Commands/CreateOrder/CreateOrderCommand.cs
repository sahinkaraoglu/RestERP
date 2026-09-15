using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(Order Order) : IRequest<Order>;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderService _orderService;

        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            => _orderService.CreateOrderAsync(request.Order);
    }
}
