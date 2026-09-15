using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(int Id) : IRequest<bool>;

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            => _orderService.DeleteOrderAsync(request.Id);
    }
}
