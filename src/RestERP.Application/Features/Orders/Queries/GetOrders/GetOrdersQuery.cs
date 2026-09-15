using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrders
{
    public record GetOrdersQuery : IRequest<IEnumerable<Order>>;

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            => _orderService.GetAllOrdersAsync();
    }
}
