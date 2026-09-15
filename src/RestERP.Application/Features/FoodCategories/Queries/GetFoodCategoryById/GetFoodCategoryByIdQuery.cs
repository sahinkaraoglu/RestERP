using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.FoodCategories.Queries.GetFoodCategoryById
{
    public record GetFoodCategoryByIdQuery(int Id) : IRequest<FoodCategory>;

    public class GetFoodCategoryByIdQueryHandler : IRequestHandler<GetFoodCategoryByIdQuery, FoodCategory>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public GetFoodCategoryByIdQueryHandler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public Task<FoodCategory> Handle(GetFoodCategoryByIdQuery request, CancellationToken cancellationToken)
            => _foodCategoryService.GetCategoryByIdAsync(request.Id);
    }
}
