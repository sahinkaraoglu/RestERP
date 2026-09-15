using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrdersByDateRange
{
    public record GetOrdersByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<Order>>;

    public class GetOrdersByDateRangeQueryHandler : IRequestHandler<GetOrdersByDateRangeQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByDateRangeQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersByDateRangeQuery request, CancellationToken cancellationToken)
            => _orderService.GetOrdersByDateRangeAsync(request.StartDate, request.EndDate);
    }
}
