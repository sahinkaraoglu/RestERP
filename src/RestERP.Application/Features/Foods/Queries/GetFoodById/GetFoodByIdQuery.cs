using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Queries.GetFoodById
{
    public record GetFoodByIdQuery(int Id) : IRequest<Food>;

    public class GetFoodByIdQueryHandler : IRequestHandler<GetFoodByIdQuery, Food>
    {
        private readonly IFoodService _foodService;

        public GetFoodByIdQueryHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<Food> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
            => _foodService.GetFoodByIdAsync(request.Id);
    }
}
