using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrdersByTableId
{
    public record GetOrdersByTableIdQuery(int TableId) : IRequest<IEnumerable<Order>>;

    public class GetOrdersByTableIdQueryHandler : IRequestHandler<GetOrdersByTableIdQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByTableIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersByTableIdQuery request, CancellationToken cancellationToken)
            => _orderService.GetOrdersByTableIdAsync(request.TableId);
    }
}
