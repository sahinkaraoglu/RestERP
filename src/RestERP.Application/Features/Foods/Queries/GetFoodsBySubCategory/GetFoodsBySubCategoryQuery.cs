using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Queries.GetFoodsBySubCategory
{
    public record GetFoodsBySubCategoryQuery(int SubCategoryId) : IRequest<IEnumerable<Food>>;

    public class GetFoodsBySubCategoryQueryHandler : IRequestHandler<GetFoodsBySubCategoryQuery, IEnumerable<Food>>
    {
        private readonly IFoodService _foodService;

        public GetFoodsBySubCategoryQueryHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<IEnumerable<Food>> Handle(GetFoodsBySubCategoryQuery request, CancellationToken cancellationToken)
            => _foodService.GetFoodsBySubCategoryAsync(request.SubCategoryId);
    }
}
