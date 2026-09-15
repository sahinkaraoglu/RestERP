using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.FoodCategories.Queries.GetFoodCategories
{
    public record GetFoodCategoriesQuery : IRequest<IEnumerable<FoodCategory>>;

    public class GetFoodCategoriesQueryHandler : IRequestHandler<GetFoodCategoriesQuery, IEnumerable<FoodCategory>>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public GetFoodCategoriesQueryHandler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public Task<IEnumerable<FoodCategory>> Handle(GetFoodCategoriesQuery request, CancellationToken cancellationToken)
            => _foodCategoryService.GetAllCategoriesAsync();
    }
}
