using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.FoodCategories.Commands.CreateFoodCategory
{
    public record CreateFoodCategoryCommand(FoodCategory Category) : IRequest<FoodCategory>;

    public class CreateFoodCategoryCommandHandler : IRequestHandler<CreateFoodCategoryCommand, FoodCategory>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public CreateFoodCategoryCommandHandler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public Task<FoodCategory> Handle(CreateFoodCategoryCommand request, CancellationToken cancellationToken)
            => _foodCategoryService.CreateCategoryAsync(request.Category);
    }
}
