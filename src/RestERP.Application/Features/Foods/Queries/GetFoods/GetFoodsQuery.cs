using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Queries.GetFoods
{
    public record GetFoodsQuery : IRequest<IEnumerable<Food>>;

    public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, IEnumerable<Food>>
    {
        private readonly IFoodService _foodService;

        public GetFoodsQueryHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<IEnumerable<Food>> Handle(GetFoodsQuery request, CancellationToken cancellationToken)
            => _foodService.GetAllFoodsAsync();
    }
}
