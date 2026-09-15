using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrdersByDate
{
    public record GetOrdersByDateQuery(DateTime Date) : IRequest<IEnumerable<Order>>;

    public class GetOrdersByDateQueryHandler : IRequestHandler<GetOrdersByDateQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByDateQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<IEnumerable<Order>> Handle(GetOrdersByDateQuery request, CancellationToken cancellationToken)
            => _orderService.GetOrdersByDateAsync(request.Date);
    }
}
