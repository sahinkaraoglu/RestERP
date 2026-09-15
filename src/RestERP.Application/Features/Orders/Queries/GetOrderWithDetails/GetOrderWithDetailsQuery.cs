using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrderWithDetails
{
    public record GetOrderWithDetailsQuery(int Id) : IRequest<Order>;

    public class GetOrderWithDetailsQueryHandler : IRequestHandler<GetOrderWithDetailsQuery, Order>
    {
        private readonly IOrderService _orderService;

        public GetOrderWithDetailsQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<Order> Handle(GetOrderWithDetailsQuery request, CancellationToken cancellationToken)
            => _orderService.GetOrderWithDetailsAsync(request.Id);
    }
}
