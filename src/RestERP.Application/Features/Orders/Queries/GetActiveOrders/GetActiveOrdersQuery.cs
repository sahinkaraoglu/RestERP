using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetActiveOrders
{
    public record GetActiveOrdersQuery : IRequest<IEnumerable<Order>>;

    public class GetActiveOrdersQueryHandler : IRequestHandler<GetActiveOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetActiveOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
            => _orderService.GetActiveOrdersAsync();
    }
}
