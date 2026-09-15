using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(int Id) : IRequest<Order>;

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            => _orderService.GetOrderByIdAsync(request.Id);
    }
}
