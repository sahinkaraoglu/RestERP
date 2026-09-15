using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Queries.GetFoodsByCategory
{
    public record GetFoodsByCategoryQuery(int CategoryId) : IRequest<IEnumerable<Food>>;

    public class GetFoodsByCategoryQueryHandler : IRequestHandler<GetFoodsByCategoryQuery, IEnumerable<Food>>
    {
        private readonly IFoodService _foodService;

        public GetFoodsByCategoryQueryHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<IEnumerable<Food>> Handle(GetFoodsByCategoryQuery request, CancellationToken cancellationToken)
            => _foodService.GetFoodsByCategoryAsync(request.CategoryId);
    }
}
