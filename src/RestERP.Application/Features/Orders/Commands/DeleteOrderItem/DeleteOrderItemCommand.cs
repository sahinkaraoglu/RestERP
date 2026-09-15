using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Orders.Commands.DeleteOrderItem
{
    public record DeleteOrderItemCommand(int OrderItemId) : IRequest<bool>;

    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, bool>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderItemCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<bool> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
            => _orderService.DeleteOrderItemAsync(request.OrderItemId);
    }
}
